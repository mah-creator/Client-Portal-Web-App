namespace ClientPortalApi.Models
{
    public enum ProjectStatus { Active, Archived, Deleted }

    public class Project
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string OwnerId { get; set; } = null!;
        public DateTime? DueDate { get; set; }
        public ProjectStatus Status { get; set; } = ProjectStatus.Active;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public List<ProjectMember> Members { get; set; } = new();
        public List<TaskItem> Tasks { get; set; } = new();
    }
}
