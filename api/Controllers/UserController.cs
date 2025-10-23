using ClientPortalApi.Data;
using ClientPortalApi.DTOs;
using ClientPortalApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Security.Claims;
using System.Text.RegularExpressions;
using TaskStatus = ClientPortalApi.Models.TaskStatus;

namespace ClientPortalApi.Controllers;

[ApiController]
[Route("api/users")]
[Authorize]
public class UserController(AppDbContext dbContext, IWebHostEnvironment env) : ControllerBase
{
	[HttpGet("profile")]
	[ProducesResponseType(typeof(ProfileDto), 200)]
	[ProducesResponseType(typeof(string), 400)]
	public async Task<IActionResult> GetProfile()
	{
		var user = GetCurrentUser();
		if (user == null) return Unauthorized();

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
	[ProducesResponseType(typeof(string), 400)]
	[ProducesResponseType(typeof(ProfileDto), 200)]
	public async Task<IActionResult> UpdateProfile([FromBody]UpdateProfileDto request)
	{
		var user = GetCurrentUser();
		if (user == null) return Unauthorized();

		// validate phone number format if not null to empty
		if(!string.IsNullOrEmpty(request.Phone))
        {
			if (!Regex.IsMatch(request.Phone, @"^\+?\d{1,15}$"))
			{
				return BadRequest("Invalid phone number format.");
			}
			user.Profile.Phone = request.Phone;
        }
		if(!string.IsNullOrEmpty(request.Name)) user.Name = request.Name;
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
	[ProducesResponseType(typeof(string), 400)]
	[ProducesResponseType(typeof(ProfileDto), 200)]
	public async Task<IActionResult> UploadAvatar(IFormFile file)
	{
		var user = GetCurrentUser();
		if (user == null) return Unauthorized();

		var filePath = $"/avatars/{user.Id}{Path.GetExtension(file.FileName)}";

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
	[HttpGet("stats")]
	public async Task<IActionResult> GetUserStats()
	{
		var user = GetCurrentUser();
		if (user == null) return Unauthorized();

		var stats = user.Role switch
		{
			Role.Admin => await GetAdminStats(user),
			Role.Customer => await GetCustomerStats(user),
			Role.Freelancer => await GetFreelancerStats(user),
			_ => null
		};

		return Ok(stats);
	}

	private async Task<UserStatsDto> GetFreelancerStats(User user)
	{
		var projects = dbContext.Projects
			.Include(p => p.Tasks).Include(p => p.Members)
			.Where(p => p.Members.Any(m => m.UserId == user.Id && m.Role == MemberRole.Collaborator))
			.GroupBy(p => new { projectId = p.Id, completedTasks = p.Tasks.Where(t => t.Status == TaskStatus.Done).Count() })
			.Select(g => g.Key);

		return new UserStatsDto
		(
			ProjectsCount: await projects.CountAsync(),
			TasksCompleted: await projects.SumAsync(p => p.completedTasks),
			FilesUploaded: await dbContext.Files.CountAsync(f => f.UploaderId == user.Id)
		);
	}

	private async Task<UserStatsDto> GetCustomerStats(User user)
	{
		var projects = dbContext.Projects
			.Include(p => p.Tasks).Include(p => p.Members)
			.Where(p => p.Members.Any(m => m.UserId == user.Id && m.Role == MemberRole.Viewer))
			.GroupBy(p => new { projectId = p.Id, completedTasks = p.Tasks.Where(t => t.Status == TaskStatus.Done).Count() })
			.Select(g => g.Key);

		return new UserStatsDto
		(
			ProjectsCount: await projects.CountAsync(),
			TasksCompleted: await projects.SumAsync(p => p.completedTasks),
			FilesUploaded: await dbContext.Files.CountAsync(f => f.UploaderId == user.Id)
		);
	}

	private async Task<UserStatsDto> GetAdminStats(User user)
	{

		return new UserStatsDto
		(
			ProjectsCount: await dbContext.Projects.CountAsync(),
			TasksCompleted: await dbContext.TaskItems.CountAsync(t => t.Status == TaskStatus.Done),
			FilesUploaded: await dbContext.Files.CountAsync()
		);
	}

	private User? GetCurrentUser()
	{
		var userId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
		if (userId == null)
			return null;

		var user = dbContext.Users.Include(x => x.Profile).FirstOrDefault(u => u.Id == userId);
		return user;
	}
}
