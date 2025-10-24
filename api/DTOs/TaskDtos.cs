namespace ClientPortalApi.DTOs
{
    public record CreateTaskDto(string Title, string? Description, DateTime? DueDate);
    public record UpdateTaskStatusDto(string Status);
    public record TaskResponse(string Id, string ProjectId, string Title, string Description, string Status, DateTime CreatedAt, DateTime UpdatedAt, List<CommentResponse> Comments, DateTime? DueDate=null, string? Assignee=null, bool IsOverdue=false, int FilesCount=0);
    public record CommentResponse(string Id, string Author, string Message, DateTime Time);
}
