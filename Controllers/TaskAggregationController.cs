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
    [Route("api/tasks")]
    [Authorize]
    public class TasksAggregationController : ControllerBase
    {
        private readonly AppDbContext _db;
        public TasksAggregationController(AppDbContext db) { _db = db; }
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

        [HttpGet("pending")]
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
