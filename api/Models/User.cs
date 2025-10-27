using System.ComponentModel.DataAnnotations;

namespace ClientPortalApi.Models
{
    public enum Role { Admin, Freelancer, Customer }

    public class User
    {
		public User()
		{
            Id = Guid.NewGuid().ToString();
            Profile = new Profile { Id = Id };
		}
		[Key]
        public string Id { get; set; }
        public string? Name { get; set; }
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = "";
        public Role Role { get; set; } = Role.Customer;
        public bool IsSuspended { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        [Required]
        public Profile Profile { get; set; }
		public List<ProjectInvitation> Invitations { get; set; }
        public List<Notification> Notifications { get; set; }
	}

    public class Profile
    {
        public string Id { get; set; }
        public string? Bio { get; set; }
        public string? Phone {  get; set; }
        public string? AvatarPath { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    }
}
