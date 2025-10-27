using ClientPortalApi.DTOs;

namespace ClientPortalApi.Models;

public enum NotificationStatus
{
	Read, NotRead, Deleted
}

public class Notification
{
	public string Id { get; set; } = Guid.NewGuid().ToString();
	public string UserId { get; set; } = default!;
	public string Type { get; set; } = NotificationType.Info;
	public string Title { get; set; } = default!;
	public string Message { get; set; } = default!;
	public DateTime Timestamp { get; set; } = DateTime.UtcNow;
	public string? ActionUrl { get; set; } = null;
	public NotificationStatus Status { get; set; } = NotificationStatus.NotRead;
}