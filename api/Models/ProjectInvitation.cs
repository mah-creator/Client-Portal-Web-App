using System.Net.NetworkInformation;

namespace ClientPortalApi.Models;

public enum InvitationStatus
{
	Pending,
	Accepted,
	Declined,
	Expired
}

public class ProjectInvitation
{
	public ProjectInvitation()
	{
		SentAt = DateTime.UtcNow;
		// TODO: make expiration configurable
		ExpiresAt = SentAt.AddDays(7);
	}
	public string Id { get; set; } = Guid.NewGuid().ToString();
	public string ProjectId { get; set; } = default!;
	public string InviterId { get; set; } = default!;
	public string InviteeId { get; set; } = default!;
	public MemberRole InviteeRole { get; set; }
	private InvitationStatus _status = InvitationStatus.Pending;
	public InvitationStatus Status
	{
		set => (_status, RespondedAt) = (value, DateTime.UtcNow);
		get => _status;
	}
	public DateTime SentAt { get; set; }
	public DateTime? RespondedAt { get; set; }
	public DateTime ExpiresAt { get; set; }
	public User Invitee { get; set; } = default!;
	public User Inviter { get; set; } = default!;
	public Project Project { get; set; }
}
