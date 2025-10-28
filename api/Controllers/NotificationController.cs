using ClientPortalApi.Data;
using ClientPortalApi.DTOs;
using ClientPortalApi.Models;
using ClientPortalApi.Paging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ClientPortalApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NotificationController(AppDbContext db) : ControllerBase
{
	[HttpGet]
	[ProducesResponseType(typeof(PagedList<NotificationDto>), StatusCodes.Status200OK)]
	public async Task<IActionResult> GetNotifications(int? page, int? pageSize)
	{
		var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
		if (userId == null) return Unauthorized();

		var notifications = db.Notifications
			.Where(n => n.UserId == userId && (n.Status == NotificationStatus.NotRead))
			.OrderByDescending(n => n.Timestamp)
			.AsNoTracking()
			.Select(n => new NotificationDto
			{
				Id = n.Id,
				Type = n.Type,
				Title = n.Title,
				Message = n.Message,
				Timestamp = n.Timestamp,
				IsRead = n.Status == NotificationStatus.Read,
				Metadata = n.Metadata
			});

		return Ok(PagedList<NotificationDto>.CreatePagedList(notifications, page, pageSize));
	}

	[HttpPatch("{id}/read")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(typeof(InvitationDto), StatusCodes.Status200OK)]
	public async Task<IActionResult> MarkAsRead(string id)
	{
		var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
		if (userId == null) return Unauthorized();
		var notification = await db.Notifications.FindAsync(id);
		if (notification == null || notification.UserId != userId)
		{
			return NotFound();
		}
		notification.Status = NotificationStatus.Read;
		await db.SaveChangesAsync();
		return Ok(new NotificationDto
		{
			Id = notification.Id,
			Type = notification.Type,
			Title = notification.Title,
			Message = notification.Message,
			Timestamp = notification.Timestamp,
			IsRead = notification.Status == NotificationStatus.Read,
			Metadata = notification.Metadata
		});
	}
	[HttpPatch("read-all")]
	public async Task<IActionResult> MarkAllRead()
	{
		var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
		if (userId == null) return Unauthorized();

		db.Notifications
			.Where(n => n.UserId == userId)
			.Where(n => n.Status == NotificationStatus.NotRead)
			.ExecuteUpdate(n => n.SetProperty(n => n.Status, NotificationStatus.Read));

		return Ok();
	}
}
