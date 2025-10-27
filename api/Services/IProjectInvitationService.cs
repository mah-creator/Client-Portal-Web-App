using ClientPortalApi.Models;

namespace ClientPortalApi.Services;

public interface IProjectInvitationService
{
	Task SendInvitationAsync(string projectId, string inviterId, string inviteeId, MemberRole role);
	Task AcceptInvitationAsync(string invitationId);

	Task DeclineInvitationAsynt(string invitationId);
}
