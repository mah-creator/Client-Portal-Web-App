using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClientPortalApi.Data;
using ClientPortalApi.Models;
using ClientPortalApi.DTOs;
using ClientPortalApi.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TaskStatus = ClientPortalApi.Models.TaskStatus;
using System.Xml.Linq;
using System.Linq.Expressions;
using System.Globalization;
using System.Collections;
using ClientPortalApi.Services.Notifications;
using ClientPortalApi.Paging;
using Microsoft.CodeAnalysis;

namespace ClientPortalApi.Controllers
{
    [ApiController]
    [Route("api/projects/{projectId}/[controller]")]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly INotificationHubService _notifications;
		private readonly object _notify;

		public TasksController(AppDbContext db, INotificationHubService notifications) { _db = db; _notifications = notifications; }

        [HttpGet]
        [ProducesResponseType(typeof(PagedList<TaskResponse>), 200)]
        public async Task<IActionResult> List(string projectId, int? page, int? pageSize)
        {
            var tasks = _db.TaskItems.OrderBy(t => t.CreatedAt).Where(t => t.ProjectId == projectId)
                .Select(t =>
                    new TaskResponse(
                        t.Id, t.ProjectId, t.Title, t.Description??"", Enum.GetName(t.Status)!, t.CreatedAt, DateTime.Now,
                        _db.Comments.OrderBy(c => c.CreatedAt).Include(c => c.User).Where(c => c.TaskId == t.Id).Select(c =>
                            new CommentResponse(c.Id.ToString(), c.User.Name!, c.Body, c.CreatedAt, c.TaskId)).ToList(), t.DueDate,
                        _db.Users.FirstOrDefault(u => u.Id == _db.Projects.FirstOrDefault(p => p.Id == projectId)!.OwnerId)!.Name,
                        t.DueDate.HasValue && t.Status != TaskStatus.Done && t.DueDate.Value < DateTime.UtcNow, _db.Files.Where(f => f.TaskId == t.Id).Count()));

            return Ok(PagedList<TaskResponse>.CreatePagedList(tasks, page, pageSize));
        }

        [HttpPost]
        public async Task<IActionResult> Create(string projectId, [FromBody] CreateTaskDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();
            var t = new TaskItem
            {
                ProjectId = projectId,
                CreatorId = userId,
                Title = dto.Title,
                Description = dto.Description,
                DueDate = dto.DueDate
            };
            _db.TaskItems.Add(t);
            await _db.SaveChangesAsync();

            var creator = _db.Users.Find(userId);
            var project = _db.Projects.Find(projectId);

			await _notifications.SendNotificationToUsers(_db.ProjectMembers.Where(p => p.ProjectId == projectId)
				.Select(p => p.UserId).Except([userId])!,
				new NotificationDto
				{
					Title = "New task was created",
                    Message = $"{creator?.Name?? creator?.Email?? "Freelancer"} created task '{dto.Title}' for project {project?.Title}",
                    Type = NotificationType.Info,
                    Metadata = new ResourceMetadata
                    {
						ProjectId = projectId,
						ResourceType = ResourceType.Task,
						ResourceId = t.Id,
                        TaskId = t.Id
					}
				});

			return CreatedAtAction(nameof(Get), new { projectId = projectId, id = t.Id }, t);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string projectId, string id)
        {
            var t = await _db.TaskItems.FirstOrDefaultAsync(x => x.Id == id && x.ProjectId == projectId);
            if (t == null) return NotFound();
            return Ok(t);
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(string projectId, string id, [FromBody] UpdateTaskStatusDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();
            var user = _db.Users.Find(userId);

            var t = await _db.TaskItems.FirstOrDefaultAsync(x => x.Id == id && x.ProjectId == projectId);
            if (t == null) return NotFound();
            if (Enum.TryParse<TaskStatus>(dto.Status, ignoreCase: true , out var st))
            {
                t.Status = st;
                await _db.SaveChangesAsync();
                var project = _db.Projects.Find(t.ProjectId);
                await _notifications.SendNotificationToUsers(_db.ProjectMembers.Where(p => p.ProjectId == projectId).Select(p => p.UserId).Except([userId!]),
                    new NotificationDto
                    {
                        Title = "Task progress",
                        Message = $"{user?.Name ?? user!.Email} updated task '{t.Title}' in project {project!.Title}, it's now {Enum.GetName(t.Status)?.ToLower()?.Replace('_', ' ')}",
                        Type = NotificationType.Info,
						Metadata = new ResourceMetadata
						{
							ProjectId = projectId,
							ResourceType = ResourceType.Task,
							ResourceId = t.Id,
							TaskId = t.Id
						}
					});
                return Ok(t);
            }
            return BadRequest("Invalid status");
        }
        [HttpPost("{id}/comment")]
        public async Task<IActionResult> PostComment(string id, [FromBody] string comment)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();
            var user = _db.Users.Find(userId);

            var t = await _db.TaskItems.FirstOrDefaultAsync(t => t.Id == id);

            if (t == null) return NotFound();
            var newComment = new Comment
            {
                TaskId = id,
                UserId = userId,
                Body = comment,
            };

            var project = _db.Projects.Find(t.ProjectId);

            _db.Comments.Add(newComment);
            await _db.SaveChangesAsync();

			await _notifications.SendNotificationToUsers(_db.ProjectMembers.Where(p => p.ProjectId == t.ProjectId).Select(p => p.UserId).Except([userId]),
				new NotificationDto
				{
				    Title = "New comment",
				    Message = $"{user?.Name ?? user!.Email} commented on task '{t.Title}' in project {project!.Title}",
                    Type = NotificationType.new_comment,
                    Metadata = new ResourceMetadata
                    {
                        ResourceId = newComment.Id,
						ResourceType = ResourceType.Comment,
                        ProjectId = project.Id,
                        TaskId = t.Id
					}
				});

            await _notifications.SendCommentToUsers(_db.ProjectMembers.Where(p => p.ProjectId == t.ProjectId).Select(p => p.UserId).Except([userId]),
                new CommentResponse(newComment.Id, user?.Name ?? user!.Email, newComment.Body, newComment.CreatedAt, newComment.TaskId));

			return Ok(new CommentResponse(newComment.Id.ToString(), user.Name?? user.Email, comment, newComment.CreatedAt, newComment.TaskId));
        }

        [HttpGet("completed")]
        [Authorize(Roles = nameof(Role.Freelancer))]
		[ProducesResponseType(typeof(PagedList<TaskResponse>), 200)]

		public async Task<IActionResult> GetCompletedTasks(int? page, int? pageSize)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var projects = _db.ProjectMembers.Where(mem => mem.UserId == userId && mem.Role == MemberRole.Collaborator);

            var completedTasks = projects
            .Join(_db.TaskItems.Where(t => t.Status == TaskStatus.Done),
                    mem => mem.ProjectId,
                    t => t.ProjectId,
                    (mem, t) => t)
            .AsQueryable();

            return Ok(PagedList<TaskItem>.CreatePagedList(completedTasks, page, pageSize));
        }

        [HttpGet("pending")]
        [Authorize(Roles = nameof(Role.Freelancer))]
        [ProducesResponseType(typeof(PagedList<TaskResponse>), 200)]
        public async Task<IActionResult> GetPendingTasks(int? page, int? pageSize)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var projects = _db.ProjectMembers.Where(mem => mem.UserId == userId && mem.Role == MemberRole.Collaborator);

            var pendingTasks = projects
            .Join(_db.TaskItems.Where(t => t.Status == TaskStatus.In_progress || t.Status == TaskStatus.Todo),
                    mem => mem.ProjectId,
                    t => t.ProjectId,
                    (mem, t) => t);

            return Ok(PagedList<TaskItem>.CreatePagedList(pendingTasks, page, pageSize));
        }
    }
}
