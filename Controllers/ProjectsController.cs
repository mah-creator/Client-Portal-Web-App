using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClientPortalApi.Data;
using ClientPortalApi.Models;
using ClientPortalApi.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
namespace ClientPortalApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProjectsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public ProjectsController(AppDbContext db) { _db = db; }

        [HttpGet]
        public async Task<IActionResult> List() {
            //var userId = User.Claims.ToList().Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault();
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var owned = _db.Projects.Where(p => p.OwnerId == userId);
            var memberProjIds = _db.ProjectMembers.Where(pm => pm.UserId == userId).Select(pm => pm.ProjectId);
            var member = _db.Projects.Where(p => memberProjIds.Contains(p.Id));
            var projects = await owned.Union(member).ToListAsync();
            return Ok(projects.Select(p => new ProjectDto(p.Id, p.Title, p.Description, p.OwnerId, p.Status.ToString(), p.CreatedAt)));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProjectDto dto) {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var project = new Project {
                Title = dto.Title,
                Description = dto.Description,
                OwnerId = userId,
                DueDate = dto.DueDate
            };
            _db.Projects.Add(project);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = project.Id }, new ProjectDto(project.Id, project.Title, project.Description, project.OwnerId, project.Status.ToString(), project.CreatedAt));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id) {
            var project = await _db.Projects.Include(p=>p.Members).FirstOrDefaultAsync(p => p.Id == id);
            if (project == null) return NotFound();
            return Ok(project);
        }

        [HttpPost("{id}/invite")]
        public async Task<IActionResult> Invite(string id, [FromBody] string email) {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) {
                user = new User { Email = email, Name = email.Split('@')[0] };
                _db.Users.Add(user);
                await _db.SaveChangesAsync();
            }

            if (await _db.ProjectMembers.AnyAsync(pm => pm.ProjectId == id && pm.UserId == user.Id)) {
                return BadRequest("Already invited");
            }
            _db.ProjectMembers.Add(new ProjectMember { ProjectId = id, UserId = user.Id, Role = MemberRole.Viewer });
            await _db.SaveChangesAsync();
            return Ok();
        }
    }
}
