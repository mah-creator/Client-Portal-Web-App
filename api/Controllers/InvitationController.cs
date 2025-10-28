using ClientPortalApi.Data;
using ClientPortalApi.DTOs;
using ClientPortalApi.Models;
using ClientPortalApi.Paging;
using ClientPortalApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ClientPortalApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class InvitationController(IProjectInvitationService inv, AppDbContext db) : ControllerBase
{
	[HttpGet]
	[ProducesResponseType(typeof(PagedList<InvitationDto>), StatusCodes.Status200OK)]
	public async Task<IActionResult> GetUserInvitations(int? page, int? pageSize)
	{
		var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
		if (userId == null) return Unauthorized();

		var invitations = db.Invitations.Include(i => i.Invitee)
			.Include(i => i.Inviter)
			.Include(i => i.Project)
			.Where(i => i.InviteeId == userId)
			.OrderBy(i => i.Status)
			.ThenByDescending(i => i.SentAt)
			.Select(i => CreateInvitationDto(i));

		return Ok(PagedList<InvitationDto>.CreatePagedList(invitations, page, pageSize));
	}

	[HttpGet("status/{status}")]
	public async Task<IActionResult> GetInvitationsByStatus(string status, int? page, int? pageSize)
	{
		var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
		if (userId == null) return Unauthorized();

		if (!Enum.TryParse(status, ignoreCase: true, out InvitationStatus enumStatus))
			return BadRequest("Invalid invitation status");

		var invitations = db.Invitations.Include(i => i.Invitee)
			.Include(i => i.Inviter)
			.Include(i => i.Project)
			.Where(i => i.InviteeId == userId && i.Status == enumStatus)
			.OrderBy(i => i.Status)
			.ThenByDescending(i => i.SentAt)
			.Select(i => CreateInvitationDto(i));

		return Ok(PagedList<InvitationDto>.CreatePagedList(invitations, page, pageSize));
	}

	[HttpPut("{invitationId}/accept")]
	[ProducesResponseType(typeof(InvitationDto), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> AcceptInvitation(string invitationId)
	{
		try
		{
			await inv.AcceptInvitationAsync(invitationId);
		}
		catch(InvalidOperationException ex)
		{
			return BadRequest(ex.Message);
		}

		var invitation = db.Invitations.Include(i => i.Inviter).Include(i => i.Project).First(i => i.Id == invitationId);

		return Ok(CreateInvitationDto(invitation));
	}

	[HttpPut("{invitationId}/decline")]
	[ProducesResponseType(typeof(InvitationDto), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> DeclineInvitation(string invitationId)
	{
		try
		{
			await inv.DeclineInvitationAsyn(invitationId);
		}
		catch (InvalidOperationException ex)
		{
			return BadRequest(ex.Message);
		}

		var invitation = db.Invitations.Include(i => i.Inviter).Include(i => i.Project).First(i => i.Id == invitationId);

		return Ok(CreateInvitationDto(invitation));
	}

	private static InvitationDto CreateInvitationDto(ProjectInvitation invitation) =>
		new InvitationDto(
			invitation.Id,
			invitation.ProjectId,
			invitation.Project.Title,
			new UserDto(invitation.Inviter.Id, invitation.Inviter.Email, invitation.Inviter.Name!, Enum.GetName(invitation.Inviter.Role)!),
			invitation.SentAt,
			Enum.GetName(invitation.Status)!,
			invitation.ExpiresAt <= DateTime.UtcNow);
}
