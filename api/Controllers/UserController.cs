using ClientPortalApi.Data;
using ClientPortalApi.DTOs;
using ClientPortalApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Security.Claims;

namespace ClientPortalApi.Controllers;

[ApiController]
[Route("api/users")]
[Authorize]
public class UserController(AppDbContext dbContext, IWebHostEnvironment env) : ControllerBase
{
	[HttpGet("profile")]
	[ProducesResponseType(typeof(ProfileDto), 200)]
	[ProducesResponseType(typeof(ProblemDetails), 400)]
	public async Task<IActionResult> GetProfile()
	{
		var userId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
		if (userId == null)
			return Problem(title: "Invalid user");

		var user = dbContext.Users.Include(x => x.Profile).FirstOrDefault(u => u.Id == userId);

		if (user == null) return Problem(title: "User wans't found");

		return Ok(new ProfileDto
		(
			Id: user.Profile.Id,
			Name: user.Name!,
			Email: user.Email,
			Bio: user.Profile.Bio!,
			Phone: user.Profile.Phone!,
			AvatarUrl: user.Profile.AvatarPath!,
			CreatedAt: user.Profile.CreatedAt,
			UpdatedAt: user.Profile.UpdatedAt
		));
	}

	[HttpPut("profile")]
	[ProducesResponseType(typeof(ProblemDetails), 400)]
	[ProducesResponseType(typeof(ValidationProblemDetails), 400)]
	[ProducesResponseType(typeof(ProfileDto), 200)]
	public async Task<IActionResult> UpdateProfile([FromBody]UpdateProfileDto request)
	{
		var userId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
		if (userId == null)
			return Problem(title: "Invalid user");

		var user = dbContext.Users.Include(x => x.Profile).FirstOrDefault(u => u.Id == userId);

		if (user == null) return Problem(title: "User wasn't found");

		if (!ModelState.IsValid)
		{
			return ValidationProblem(ModelState);
		}

		if(!string.IsNullOrEmpty(request.Name)) user.Name = request.Name;
		if(!string.IsNullOrEmpty(request.Phone)) user.Profile.Phone = request.Phone;
		if(!string.IsNullOrEmpty(request.Bio)) user.Profile.Bio = request.Bio;

		await dbContext.SaveChangesAsync();

		return Ok(new ProfileDto
		(
			Id: user.Profile.Id,
			Name: user.Name!,
			Email: user.Email,
			Bio: user.Profile.Bio!,
			Phone: user.Profile.Phone!,
			AvatarUrl: user.Profile.AvatarPath!,
			CreatedAt: user.Profile.CreatedAt,
			UpdatedAt: user.Profile.UpdatedAt
		));
	}

	[HttpPost("avatar")]
	[ProducesResponseType(typeof(ValidationProblemDetails), 400)]
	[ProducesResponseType(typeof(ProblemDetails), 400)]
	[ProducesResponseType(typeof(ProfileDto), 200)]
	public async Task<IActionResult> UploadAvatar(IFormFile file)
	{
		var userId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
		if (userId == null)
			return Problem(title: "Invalid user");

		var user = dbContext.Users.Include(x => x.Profile).FirstOrDefault(u => u.Id == userId);

		if (user == null) return Problem(title: "User wasn't found");

		var filePath = $"/avatars/{userId}{Path.GetExtension(file.FileName)}";

		var absoluteFilePath = $"{env.WebRootPath}{Path.DirectorySeparatorChar}{filePath}";

		StreamWriter streamWriter = new StreamWriter(absoluteFilePath);
		await file.CopyToAsync(streamWriter.BaseStream);
		streamWriter.Close();

		user.Profile.AvatarPath = filePath;

		await dbContext.SaveChangesAsync();

		return Ok(new ProfileDto
		(
			Id: user.Profile.Id,
			Name: user.Name!,
			Email: user.Email,
			Bio: user.Profile.Bio!,
			Phone: user.Profile.Phone!,
			AvatarUrl: user.Profile.AvatarPath!,
			CreatedAt: user.Profile.CreatedAt,
			UpdatedAt: user.Profile.UpdatedAt
		));
	}
}
