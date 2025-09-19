using ClientPortalApi.Models;

namespace ClientPortalApi.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(AppDbContext db)
        {
            if (!db.Users.Any())
            {
                var admin = new User { Email = "admin@local", Name = "Admin", Role = Role.Admin };
                var freelancer = new User { Email = "freelancer@local", Name = "Freelancer", Role = Role.Freelancer };
                var customer = new User { Email = "customer@local", Name = "Customer", Role = Role.Customer };
                db.Users.AddRange(admin, freelancer, customer);
                await db.SaveChangesAsync();

                var p = new Project { Title = "Demo Project", Description = "Seeded", OwnerId = freelancer.Id };
                db.Projects.Add(p);
                await db.SaveChangesAsync();

                db.ProjectMembers.Add(new ProjectMember { ProjectId = p.Id, UserId = customer.Id, Role = MemberRole.Viewer });
                db.TaskItems.Add(new TaskItem { ProjectId = p.Id, CreatorId = freelancer.Id, Title = "Initial Task" });
                await db.SaveChangesAsync();
            }
        }
    }
}
