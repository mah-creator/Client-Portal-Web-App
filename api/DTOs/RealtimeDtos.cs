namespace ClientPortalApi.DTOs;

public static class ResourceType
{
	public const string Project = "project";
	public const string Task = "task";
	public const string Comment = "comment";
	public const string File = "file";
	public const string Invitation = "invitation";
}

public static class NotificationType
{
	public static string task_status_changed = "task_status_changed";
	public static string project_status_updated = "project_status_updated";
	public static string invited_to_project = "invited_to_project";
	public static string invitation_accepted = "invitation_accepted";
	public static string invitation_declined = "invitation_declined";
	public static string new_comment = "new_comment";
	public static string Info = "info";
}

public struct NotificationDto
{
	public NotificationDto() {}
	public string Id { get; internal set; } = Guid.NewGuid().ToString();
	public string Type { get; set; }
	public string Title { get; set; }
	public string Message { get; set; }
	public DateTime Timestamp { get; set; } = DateTime.UtcNow;
	public string? ActionUrl { get; set; } = null;
	public bool IsRead { get; internal set; }
	public ResourceMetadata Metadata { get; set; }
}

public struct ResourceMetadata
{
	public string ResourceType { get; set; }
	public string ResourceId { get; set; }
	public string ProjectId { get; set; }
	public string TaskId { get; set; }
}