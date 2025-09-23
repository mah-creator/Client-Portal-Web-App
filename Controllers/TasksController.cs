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

namespace ClientPortalApi.Controllers
{
    [ApiController]
    [Route("api/projects/{projectId}/[controller]")]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly INotificationService _notifications;
        public TasksController(AppDbContext db, INotificationService notifications) { _db = db; _notifications = notifications; }

        [HttpGet]
        public async Task<IActionResult> List(string projectId)
        {
            var tasks = _db.TaskItems.OrderBy(t => t.CreatedAt).Where(t => t.ProjectId == projectId)
                .Select(t =>
                    new TaskResponse(
                        t.Id, t.ProjectId, t.Title, t.Description, Enum.GetName(t.Status), t.CreatedAt, DateTime.Now,
                        _db.Comments.OrderBy(c => c.CreatedAt).Include(c => c.User).Where(c => c.TaskId == t.Id).Select(c =>
                            new CommentResponse(c.Id.ToString(), c.User.Name, c.Body, c.CreatedAt)).ToList(), t.DueDate,
                        _db.Users.FirstOrDefault(u => u.Id == _db.Projects.FirstOrDefault(p => p.Id == projectId).OwnerId).Name));

            return Ok(tasks);
        }

        [HttpPost]
        public async Task<IActionResult> Create(string projectId, [FromBody] CreateTaskDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
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

            await _notifications.NotifyProjectMembersAsync(projectId, $"{userId} created a task {t.Title}");
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
            var t = await _db.TaskItems.FirstOrDefaultAsync(x => x.Id == id && x.ProjectId == projectId);
            if (t == null) return NotFound();
            if (Enum.TryParse<TaskStatus>(dto.Status, out var st))
            {
                t.Status = st;
                await _db.SaveChangesAsync();
                return Ok(t);
            }
            return BadRequest("Invalid status");
        }
        [HttpPost("{id}/comment")]
        public async Task<IActionResult> PostComment(string id, [FromBody] string comment)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();
            var t = await _db.TaskItems.FirstOrDefaultAsync(t => t.Id == id);
            if (t == null) return NotFound();
            var newComment = new Comment
            {
                TaskId = id,
                UserId = userId,
                Body = comment,
                CreatedAt = DateTime.Now,
            };


            _db.Comments.Add(newComment);
            await _db.SaveChangesAsync();
            return Ok(new CommentResponse(newComment.Id.ToString(), _db.Users.FirstOrDefault(u => u.Id == userId)?.Name, comment, newComment.CreatedAt));
        }

        [HttpGet("completed")]
        [Authorize(Roles = nameof(Role.Freelancer))]
        public async Task<IActionResult> GetCompletedTasks()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var projects = _db.ProjectMembers.Where(mem => mem.UserId == userId && mem.Role == MemberRole.Collaborator);

            var completedTasks = projects
            .Join(_db.TaskItems.Where(t => t.Status == TaskStatus.Done),
                    mem => mem.ProjectId,
                    t => t.ProjectId,
                    (mem, t) => t);

            return Ok(completedTasks);
        }

        [HttpGet]
        [Route("pending")]
        [Authorize(Roles = nameof(Role.Freelancer))]
        public async Task<IActionResult> GetPendingTasks()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var projects = _db.ProjectMembers.Where(mem => mem.UserId == userId && mem.Role == MemberRole.Collaborator);

            var pendingTasks = projects
            .Join(_db.TaskItems.Where(t => t.Status == TaskStatus.InProgress || t.Status == TaskStatus.Todo),
                    mem => mem.ProjectId,
                    t => t.ProjectId,
                    (mem, t) => t);

            return Ok(pendingTasks);
        }
    }
}
