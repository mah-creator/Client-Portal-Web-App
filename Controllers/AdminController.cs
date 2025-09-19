using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ClientPortalApi.Data;
using System.IdentityModel.Tokens.Jwt;

namespace ClientPortalApi.Controllers
{
    [ApiController]
    [Route("api/admin")]
    [Authorize]
    public class AdminController : ControllerBase
    {
        private readonly AppDbContext _db;
        public AdminController(AppDbContext db) { _db = db; }

        private bool IsAdmin => User.Claims.Any(c => c.Type == "role" && c.Value == "Admin");

        [HttpPost("suspend/{userId}")]
        public async Task<IActionResult> Suspend(string userId) {
            if (!IsAdmin) return Forbid();
            var u = await _db.Users.FindAsync(userId);
            if (u == null) return NotFound();
            u.IsSuspended = true;
            await _db.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("unsuspend/{userId}")]
        public async Task<IActionResult> Unsuspend(string userId) {
            if (!IsAdmin) return Forbid();
            var u = await _db.Users.FindAsync(userId);
            if (u == null) return NotFound();
            u.IsSuspended = false;
            await _db.SaveChangesAsync();
            return Ok();
        }
    }
}
