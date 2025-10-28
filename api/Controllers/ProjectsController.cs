using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClientPortalApi.Data;
using ClientPortalApi.Models;
using ClientPortalApi.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TaskStatus = ClientPortalApi.Models.TaskStatus;
using ClientPortalApi.Services;
using ClientPortalApi.Paging;
using ClientPortalApi.Services.Notifications;
namespace ClientPortalApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProjectsController : ControllerBase
    {
        private readonly AppDbContext _db;
		private readonly IProjectInvitationService _inv;
		private readonly INotificationHubService _notify;

		public ProjectsController(AppDbContext db, IProjectInvitationService inv, INotificationHubService notify) { _db = db; _inv = inv; _notify = notify; }

  //      [HttpGet]
  //      [ProducesResponseType(typeof(PagedList<ProjectDto>), 200)]
		//public async Task<IActionResult> List(int? page, int? pageSize)
		//{
		//	var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

  //          var projectDtos = _db.Projects
  //      .Include(p => p.Members).ThenInclude(m => m.User)
  //      .Include(p => p.Tasks)
  //      .Where(p => p.Members.Any(m => m.UserId == userId))
  //      .OrderByDescending(p => p.CreatedAt)
  //      .AsEnumerable()
  //      .Select(p =>
  //      {
  //          var totalTasks = p.Tasks.Count(t => t.Status != TaskStatus.Canceled);
  //          var completedTasks = p.Tasks.Count(t => t.Status == TaskStatus.Done);

		//	return new ProjectDto
  //          (
  //              p.Id,
  //              p.Title,
  //              p.Description,
  //              p.OwnerId,
  //              Enum.GetName(p.Status)!,
  //              p.CreatedAt,
  //              p.DueDate,
  //              totalTasks,
  //              completedTasks,
  //              p.Members.Where(m => m.Role == MemberRole.Collaborator)
  //                       .Select(m => m.User.Name)
  //                       .FirstOrDefault()!, // Gets first collaborator name or null
  //              p.Members.Where(m => m.Role == MemberRole.Viewer)
  //                       .Select(m => m.User.Name)
  //                       .FirstOrDefault()!,
  //              completedTasks == 0 ? 0 : completedTasks*100/totalTasks
  //          );
  //      });

		//	return Ok(PagedList<ProjectDto>.CreatePagedList(projectDtos.AsQueryable(), page, pageSize));
		//}

		[HttpGet()]
		[ProducesResponseType(typeof(PagedList<ProjectDto>), 200)]
		public async Task<IActionResult> Get(string? status, int? page, int? pageSize)
		{
            var enumStatus = ProjectStatus.Active;
			if (status != null && !Enum.TryParse<ProjectStatus>(status, ignoreCase: true, out enumStatus))
            {
				return BadRequest("Invalid status value");
			}

			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var filteredProjects = status != null ? _db.Projects.Where(p => p.Status == enumStatus) : _db.Projects;

			var projectDtos = filteredProjects
            .Include(p => p.Members).ThenInclude(m => m.User)
		    .Include(p => p.Tasks)
		    .Where(p => p.Members.Any(m => m.UserId == userId))
		    .OrderByDescending(p => p.CreatedAt)
		    .AsEnumerable()
		    .Select(p =>
		    {
			    var totalTasks = p.Tasks.Count(t => t.Status != TaskStatus.Canceled);
			    var completedTasks = p.Tasks.Count(t => t.Status == TaskStatus.Done);

			    return new ProjectDto
			    (
				    p.Id,
				    p.Title,
				    p.Description,
				    p.OwnerId,
				    Enum.GetName(p.Status)!,
				    p.CreatedAt,
				    p.DueDate,
				    totalTasks,
				    completedTasks,
				    p.Members.Where(m => m.Role == MemberRole.Collaborator)
						     .Select(m => m.User.Name)
						     .FirstOrDefault()!, // Gets first collaborator name or null
				    p.Members.Where(m => m.Role == MemberRole.Viewer)
						     .Select(m => m.User.Name)
						     .FirstOrDefault()!,
				    completedTasks == 0 ? 0 : completedTasks * 100 / totalTasks
			    );
		    });

			return Ok(PagedList<ProjectDto>.CreatePagedList(projectDtos.AsQueryable(), page, pageSize));
		}


		[HttpPost]
        [Authorize(Roles = nameof(Role.Freelancer))]
        public async Task<IActionResult> Create([FromBody] CreateProjectDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var project = new Project
            {
                Title = dto.Title,
                Description = dto.Description,
                OwnerId = userId!,
                DueDate = dto.DueDate
            };
            _db.Projects.Add(project);
            await _db.SaveChangesAsync();

            _db.ProjectMembers.Add(new ProjectMember
            {
                ProjectId = project.Id,
                UserId = userId!,
                Role = MemberRole.Collaborator
            });
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = project.Id }, new ProjectDto(project.Id, project.Title, project.Description, project.OwnerId, project.Status.ToString(), project.CreatedAt, project.DueDate,
                Freelancer: project.OwnerId!));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var p = await _db.Projects.Include(p => p.Members).FirstOrDefaultAsync(p => p.Id == id);
            if (p == null) return NotFound();

            return Ok(createProjectDto(p));
        }

        [HttpPost("{id}/invite")]
        public async Task<IActionResult> Invite(string id, [FromBody] string email)
        {
            var inviterId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (inviterId == null) return Unauthorized();

			var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return NotFound($"User '{email}' wasn't found");

            if(_db.ProjectMembers.Where(pm => pm.ProjectId == id && pm.Role == MemberRole.Collaborator)
				.FirstOrDefault()!.UserId != inviterId)
			{
				return BadRequest("Only collaborators can invite new members");
			}

            if(_db.ProjectMembers.Where(pm => pm.ProjectId == id && pm.Role == MemberRole.Viewer).Count() > 0)
            {
                return BadRequest("You already have a customer for this project");
            }

            if (await _db.Invitations.AnyAsync(i => 
                i.ProjectId == id && i.InviteeId == user.Id && 
                new[] { InvitationStatus.Pending, InvitationStatus.Accepted }.Contains(i.Status)))
            {
                return BadRequest("Already invited");
            }

			if (await _db.ProjectMembers.AnyAsync(pm => pm.ProjectId == id && pm.UserId == user.Id))
            {
				return BadRequest("User is already a member of the project");
			}


			await _inv.SendInvitationAsync(id, inviterId, user.Id, MemberRole.Viewer);

			return Ok();
        }

		[HttpPatch("{id}/status")]
		public async Task<IActionResult> UpdateStatus(string id, [FromBody] UpdateProjectStatusDto dto)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (userId == null) return Unauthorized();
			var user = _db.Users.Find(userId);

			var p = await _db.Projects.FirstOrDefaultAsync(x => x.Id == id && x.Id == id);
			if (p == null) return NotFound();
			if (Enum.TryParse<ProjectStatus>(dto.Status, ignoreCase: true, out var st))
			{
				p.Status = st;
				await _db.SaveChangesAsync();
				await _notify.SendNotificationToUsers(_db.ProjectMembers.Where(p => p.ProjectId == id).Select(p => p.UserId).Except([userId!]),
					new NotificationDto
					{
						Title = "Project progress",
						Message = $"{user?.Name ?? user!.Email} updated project '{p!.Title}', it's now {Enum.GetName(p.Status)?.ToLower()?.Replace('_', ' ')}",
						Type = NotificationType.Info
					});
				return Ok(createProjectDto(p));
			}
			return BadRequest("Invalid status");
		}

		private ProjectDto createProjectDto(Project p)
        {
            var totalTasks = _db.TaskItems.Where(t => t.ProjectId == p.Id).Count();
            var canceledTasks = _db.TaskItems.Where(t => t.ProjectId == p.Id)
                    .Where(t => t.Status == TaskStatus.Canceled).Count();
            var completedTasks = _db.TaskItems.Where(t => t.ProjectId == p.Id)
                    .Where(t => t.Status == TaskStatus.Done).Count();

            return new ProjectDto(
                p.Id, p.Title, p.Description, p.OwnerId, Enum.GetName(p.Status)!, p.CreatedAt, p.DueDate,
                totalTasks - canceledTasks,
                completedTasks,
                _db.Users.FirstOrDefault(u => u.Id == p.OwnerId)?.Name!,
                _db.Users.FirstOrDefault(u => u.Id ==
                    _db.ProjectMembers
                    .Where(mem => mem!.ProjectId == p.Id && mem!.Role == MemberRole.Viewer)
                    .FirstOrDefault()!.UserId
                )?.Name!,
                totalTasks == 0 ? 0 : completedTasks / ((float)totalTasks - canceledTasks));
        }
    }



}
