
using ClientPortalApi.Data;
using ClientPortalApi.DTOs;
using ClientPortalApi.Models;
using ClientPortalApi.Services.Notifications;
using Humanizer;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace ClientPortalApi.Services;

public class ProjectInvitationService(AppDbContext db, INotificationHubService notify) : IProjectInvitationService
{
	public async Task AcceptInvitationAsync(string invitationId)
	{
		var inv = await db.Invitations.FindAsync(invitationId);
		if (inv == null)
		{
			throw new InvalidOperationException("Invitation not found");
		}
		if (inv.ExpiresAt <= DateTime.UtcNow)
		{
			throw new InvalidOperationException("Invitation has expired");
		}
		if (inv.Status != InvitationStatus.Pending)
		{
			throw new InvalidOperationException("Invitation is not pending");
		}

		var customer = db.Users.Where(u => u.Id == inv.InviteeId).Select(u => new { u.Email, u.Name }).FirstOrDefault();
		var projectName = db.Projects.Where(p => p.Id == inv.ProjectId).Select(p => p.Title).FirstOrDefault();

		// add user to project members
		db.ProjectMembers.Add(new ProjectMember
		{
			ProjectId = inv.ProjectId,
			UserId = inv.InviteeId,
			Role = inv.InviteeRole,
		});

		// update invitation status
		inv.Status = InvitationStatus.Accepted;

		await db.SaveChangesAsync();

		// TODO: send notification to inviter
		await notify.SendNotificationToUser(inv.InviterId, new NotificationDto
		{
			Title = "Invitation accepted",
			Message = $"{customer?.Name ?? customer?.Email ?? "Client"} accepted your invitation to project {projectName ?? ""}",
			Type = NotificationType.invitation_accepted,
			Metadata = new ResourceMetadata
			{
				ProjectId = inv.ProjectId
			}
		});
		
	}

	public async Task DeclineInvitationAsyn(string invitationId)
	{
		var inv = db.Invitations.Find(invitationId);
		if (inv == null)
		{
			throw new InvalidOperationException("Invitation not found");
		}
		if (inv.ExpiresAt <= DateTime.UtcNow)
		{
			throw new InvalidOperationException("Invitation has expired");
		}
		if (inv.Status != InvitationStatus.Pending)
		{
			throw new InvalidOperationException("Invitation is not pending");
		}

		var customer = db.Users.Where(u => u.Id == inv.InviteeId).Select(u => new { u.Email, u.Name }).FirstOrDefault();
		var projectName = db.Projects.Where(p => p.Id == inv.ProjectId).Select(p => p.Title).FirstOrDefault();

		// update invitation status
		inv.Status = InvitationStatus.Declined;

		await db.SaveChangesAsync();

		// TODO: send notification to inviter
		await notify.SendNotificationToUser(inv.InviterId, new NotificationDto
		{
			Title = "Invitation declined",
			Message = $"{customer?.Name ?? customer?.Name?? "Client"} declined your invitation to project {projectName ?? ""}",
			Type = NotificationType.invitation_declined,
			Metadata = new ResourceMetadata
			{
				ProjectId = inv.ProjectId
			}
		});
	}

	public async Task SendInvitationAsync(string projectId, string inviterId, string inviteeId, MemberRole role)
	{
		// add entry to invitation table
		var inv = new ProjectInvitation
		{
			ProjectId = projectId,
			InviterId = inviterId,
			InviteeId = inviteeId,
			InviteeRole = role,
		};
		db.Invitations.Add(inv);

		await db.SaveChangesAsync();

		var inviter = db.Users.Where(u => u.Id == inviterId).Select(u => new { u.Email, u.Name }).FirstOrDefault();
		var projectName = db.Projects.Where(p => p.Id == projectId).Select(p => p.Title).FirstOrDefault();

		// TODO: send notification to invitee
		await notify.SendNotificationToUser(inviteeId, new NotificationDto
		{
			Title = "Project invitation",
			Message = $"{inviter?.Name ?? inviter?.Email ?? "Freelancer"} invited you to project {projectName ?? ""}",
			Type = NotificationType.invited_to_project,
			Metadata = new ResourceMetadata
			{
				ResourceId = inv.Id,
				ProjectId = projectId,
				ResourceType = ResourceType.Invitation
			}
		});
	}
}
