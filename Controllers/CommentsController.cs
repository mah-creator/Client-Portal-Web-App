using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClientPortalApi.Data;
using ClientPortalApi.Models;
using System.Security.Claims;

namespace ClientPortalApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CommentsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public CommentsController(AppDbContext db) { _db = db; }

        [HttpPost("{taskId}")]
        public async Task<IActionResult> Add(string taskId, [FromBody] string content)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!_db.TaskItems.Any(t => t.Id == taskId)) return NotFound("Task not found");

            var c = new Comment { Content = content, UserId = userId, TaskId = taskId };
            _db.Comments.Add(c);
            await _db.SaveChangesAsync();
            return Ok(c);
        }

        [HttpGet("{taskId}")]
        public async Task<IActionResult> GetByTask(string taskId)
        {
            var comments = await _db.Comments
                .Include(c => c.User)
                .Where(c => c.TaskId == taskId)
                .Select(c => new { c.Id, c.Content, c.CreatedAt, User = c.User.Name })
                .ToListAsync();
            return Ok(comments);
        }
    }
}
