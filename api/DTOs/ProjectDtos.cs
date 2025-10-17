namespace ClientPortalApi.DTOs
{
    public record CreateProjectDto(string Title, string? Description, DateTime? DueDate);
    public record ProjectDto(string Id, string Title, string? Description, string OwnerId, string Status, DateTime CreatedAt, DateTime? DueDate, int TasksTotal=0, int TasksCompleted = 0, string Freelancer=null!, string Client=null!, float Progress=0);
}
