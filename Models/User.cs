using System.ComponentModel.DataAnnotations;

namespace ClientPortalApi.Models
{
    public enum Role { Admin, Freelancer, Customer }

    public class User
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? Name { get; set; }
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = "";
        public Role Role { get; set; } = Role.Customer;
        public bool IsSuspended { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
