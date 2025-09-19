using Microsoft.AspNetCore.Mvc;
using ClientPortalApi.Data;
using ClientPortalApi.Models;
using Microsoft.EntityFrameworkCore;
using ClientPortalApi.DTOs;
using ClientPortalApi.Services;

namespace ClientPortalApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly ITokenService _token;
        private readonly IConfiguration _config;
        public AuthController(AppDbContext db, ITokenService token, IConfiguration config)
        {
            _db = db; _token = token; _config = config;
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthResponse), 200)]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.Email)) return BadRequest("Email required");

            bool demo = _config.GetValue<bool>("DemoAuth");
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == req.Email);

            if (demo) {
                if (user == null) {
                    user = new User {
                        Email = req.Email,
                        Name = req.Email.Split('@')[0],
                        Role = Role.Customer
                    };
                    user.PasswordHash = "demo";
                    _db.Users.Add(user);
                    await _db.SaveChangesAsync();
                }
                var token = _token.GenerateToken(user);
                return Ok(new AuthResponse(token, _token.ExpiresAt, new UserDto( user.Id, user.Email, user.Name, user.Role.ToString())) );
            }

            return Unauthorized();
        }
    }
}
