using ClientPortalApi.Data;
using ClientPortalApi.DTOs;
using ClientPortalApi.Hubs;
using ClientPortalApi.Models;
using Microsoft.AspNetCore.SignalR;

namespace ClientPortalApi.Services.Notifications;

// Services/NotificationHubService.cs
public interface INotificationHubService
{
	Task SendNotificationToUser(string userId, NotificationDto notification);
	Task SendNotificationToUsers(IEnumerable<string> userIds, NotificationDto notification);
	Task SendToGroup(string groupName, NotificationDto notification);
	Task SendToAll(NotificationDto notification);
	Task SendToAllExcept(string userId, NotificationDto notification);
	Task<bool> IsUserOnline(string userId);
	Task<int> GetOnlineUserCount();
	Task SendCommentToUsers(IEnumerable<string> userIds, CommentResponse comment);
}

public class NotificationHubService : INotificationHubService
{
	private readonly IHubContext<NotificationHub> _hubContext;
	private readonly ILogger<NotificationHubService> _logger;
	private readonly AppDbContext _db;

	public NotificationHubService(
		IHubContext<NotificationHub> hubContext,
		ILogger<NotificationHubService> logger,
		AppDbContext db)
	{
		_hubContext = hubContext;
		_logger = logger;
		_db = db;
	}

	public async Task SendNotificationToUser(string userId, NotificationDto notification)
	{
		try
		{

			await _db.Notifications.AddAsync(new Notification
			{
				Id = notification.Id,
				ActionUrl = notification.ActionUrl,
				Message = notification.Message,
				Title = notification.Title,
				Type = notification.Type,
				UserId = userId,
				Metadata = notification.Metadata
			});

			await _db.SaveChangesAsync();

			await _hubContext.Clients
				.Group($"user_{userId}")
				.SendAsync("ReceiveNotification", notification);

			_logger.LogInformation("Notification sent to user {UserId}. Type: {NotificationType}",
				userId, notification.Type);

		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Failed to send notification to user {UserId}", userId);
			throw;
		}
	}

	public async Task SendNotificationToUsers(IEnumerable<string> userIds, NotificationDto notification)
	{
		try
		{
			foreach (var userId in userIds)
			{

				await _db.Notifications.AddAsync(new Notification
				{
					Id = notification.Id,
					ActionUrl = notification.ActionUrl,
					Message = notification.Message,
					Title = notification.Title,
					Type = notification.Type,
					UserId = userId,
					Metadata = notification.Metadata
				});



				await _hubContext.Clients
					.Group($"user_{userId}")
					.SendAsync("ReceiveNotification", notification);

				_logger.LogInformation("Notification sent to user {UserId}. Type: {NotificationType}",
					userId, notification.Type);
			}

			_logger.LogInformation("Notification sent to {UserCount} users. Type: {NotificationType}",
				userIds.Count(), notification.Type);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Failed to send notifications");
			throw;
		}
		finally
		{
			await _db.SaveChangesAsync();

		}
	}

	public async Task SendCommentToUsers(IEnumerable<string> userIds, CommentResponse comment)
	{
		try
		{
			foreach (var userId in userIds)
			{

				await _hubContext.Clients
					.Group($"user_{userId}")
					.SendAsync("ReceiveComment", comment);

				_logger.LogInformation("Comment sent to user {UserId}. At {timestamp}",
					userId, DateTime.UtcNow);

			}
			_logger.LogInformation("Comment sent to user {UserCount}. At {timestamp}",
				userIds.Count(), DateTime.UtcNow);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Failed to send comments");
			throw;
		}
	}
	public async Task SendToGroup(string groupName, NotificationDto notification)
	{
		try
		{
			await _hubContext.Clients
				.Group(groupName)
				.SendAsync("ReceiveNotification", notification);

			_logger.LogInformation("Notification sent to group {GroupName}. Type: {NotificationType}",
				groupName, notification.Type);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Failed to send notification to group {GroupName}", groupName);
			throw;
		}
	}

	public async Task SendToAll(NotificationDto notification)
	{
		try
		{
			await _hubContext.Clients
				.All
				.SendAsync("ReceiveNotification", notification);

			_logger.LogInformation("Notification broadcast to all users. Type: {NotificationType}",
				notification.Type);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Failed to broadcast notification to all users");
			throw;
		}
	}

	public async Task SendToAllExcept(string userId, NotificationDto notification)
	{
		try
		{
			await _hubContext.Clients
				.AllExcept(new[] { GetUserConnectionId(userId) })
				.SendAsync("ReceiveNotification", notification);

			_logger.LogInformation("Notification sent to all except user {UserId}. Type: {NotificationType}",
				userId, notification.Type);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Failed to send notification to all except user {UserId}", userId);
			throw;
		}
	}

	public Task<bool> IsUserOnline(string userId)
	{
		var connectionCount = NotificationHub.GetConnectionCountForUser(userId);
		return Task.FromResult(connectionCount > 0);
	}

	public Task<int> GetOnlineUserCount()
	{
		var uniqueUsers = NotificationHub.GetActiveConnections()
			.Select(uc => uc.UserId)
			.Distinct()
			.Count();

		return Task.FromResult(uniqueUsers);
	}

	private string? GetUserConnectionId(string userId)
	{
		// This is simplified - in reality, you'd need to map user IDs to connection IDs
		var userConnection = NotificationHub.GetActiveConnections()
			.FirstOrDefault(uc => uc.UserId == userId);

		return userConnection?.ConnectionId;
	}
}