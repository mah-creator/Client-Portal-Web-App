using Microsoft.AspNetCore.Mvc;
using ClientPortalApi.Data;
using ClientPortalApi.Models;
using Microsoft.EntityFrameworkCore;
using ClientPortalApi.DTOs;
using ClientPortalApi.Services;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace ClientPortalApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly ITokenService _token;
        private readonly IConfiguration _config;
        private readonly IPasswordHasher<User> _hasher;
        private readonly IPasswordValidator<User> _validator;
        private readonly IStripeService _stripe;
        public AuthController(AppDbContext db, ITokenService token, IConfiguration config, IPasswordHasher<User> hasher, IStripeService stripe)
		{
			_db = db; _token = token; _config = config; _hasher = hasher;
			_stripe = stripe;
		}

		[HttpPost("login")]
        [ProducesResponseType(typeof(AuthResponse), 200)]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.Email)) return BadRequest("Email required");

            bool demo = _config.GetValue<bool>("DemoAuth");
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == req.Email);

            if (user == null) return Unauthorized("Invalid email");
            if (_hasher.VerifyHashedPassword(user, user.PasswordHash, req.Password) != PasswordVerificationResult.Success) return Unauthorized("Invalid password");

            var token = _token.GenerateToken(user);
            return Ok(new AuthResponse(token, _token.ExpiresAt, new UserDto( user.Id, user.Email, user.Name, user.Role.ToString())) );

        }
        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody] SignupRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.Email)) return BadRequest("Email required");
            if (_db.Users.Where(u => u.Email == req.Email).Any()) return BadRequest("Duplicate email");
            if (!Enum.TryParse<Role>(req.Role, out Role role)) return BadRequest("Invalid role");

            var user = new User
            {
                Email = req.Email,
                Name = req.Name,
                Role = role,
            };
            user.PasswordHash = _hasher.HashPassword(user, req.Password);

            var customerStripeId = await _stripe.CreateCustomerAsync(user.Email, user.Name);
            user.StripeCustomerId = customerStripeId;

            _db.Users.Add(user);

            await _db.SaveChangesAsync();
            

            return await Login(new LoginRequest(req.Email, req.Password));
		}

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest req)
        {
			var userId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (userId == null)
				return BadRequest("Invalid user");

			var user = _db.Users.Include(x => x.Profile).FirstOrDefault(u => u.Id == userId);

			if (user == null) return BadRequest("User wasn't found");

            var oldPassVerified = _hasher
                .VerifyHashedPassword(user, user.PasswordHash, req.CurrentPassword)
                .HasFlag(PasswordVerificationResult.Success);

            if (!oldPassVerified)
                return BadRequest("Incorrect password");

            var newPassHash = _hasher.HashPassword(user, req.NewPassword);
            user.PasswordHash = newPassHash;

            await _db.SaveChangesAsync();

            return Ok();
		}
	}
}
