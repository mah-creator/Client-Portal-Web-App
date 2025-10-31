using System.ComponentModel.DataAnnotations;

namespace ClientPortalApi.DTOs
{
    public record CreateProjectDto(string Title, string? Description, DateTime? DueDate, [Range(1, 1000000)] int Price, string Currency = "usd");
	public record UpdateProjectStatusDto(string Status);
	public record ProjectDto(string Id, string Title, string? Description, string OwnerId, string Status, DateTime CreatedAt, DateTime? DueDate, int TasksTotal=0, int TasksCompleted = 0, string Freelancer=null!, string Client=null!, float Progress=0);
    public record InvitationDto(string Id, string ProjectId, string ProjectTitle, UserDto Inviter, DateTime InvitationDate, string Status, bool IsExpired, int Price, string Currency);
}
