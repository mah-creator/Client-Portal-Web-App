using ClientPortalApi.DTOs;
using Microsoft.EntityFrameworkCore;

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
	public ResourceMetadata Metadata { get; set; }
}

[Owned]
public class ResourceMetadata
{
	public string? ResourceType { get; set; }
	public string? ResourceId { get; set; }
	public string? ProjectId { get; set; }
	public string? TaskId { get; set; }
}