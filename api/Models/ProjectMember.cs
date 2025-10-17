namespace ClientPortalApi.Models
{
    public enum MemberRole { Viewer, Collaborator }

    public class ProjectMember
    {
        public string ProjectId { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public MemberRole Role { get; set; } = MemberRole.Viewer;
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    }
}
