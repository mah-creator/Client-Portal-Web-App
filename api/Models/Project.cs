namespace ClientPortalApi.Models
{
    public enum ProjectStatus { Active, Completed, Deleted, Pending_Delete_Approval, Pending_Complete_Approval }

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

        // stripe specific properties that map this project to a stripe product object
        public string? StripeProductId { get; set; } = null!;
        public string? StripePriceId { get; set; } = null!;
        public string? StripeCheckoutSessionId { get; set; } = null!;
        public bool Paid { get; set; } = false;
        public int Price { get; set; } = default!;
		public string Currency { get; set; } = default!;
	}
}
