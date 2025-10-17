using ClientPortalApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ClientPortalApi.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(AppDbContext db, [FromServices] IPasswordHasher<User> hasher)
        {
            if (!db.Users.Any())
            {
				var admin = new User { Email = "admin@local.com", Name = "Yousif Khalil", Role = Role.Admin };
				admin.PasswordHash = hasher.HashPassword(admin, "123");
				var freelancer = new User { Email = "freelancer@local.com", Name = "Abedalla Jamal", Role = Role.Freelancer };
				freelancer.PasswordHash = hasher.HashPassword(freelancer, "123");
				var customer = new User { Email = "customer@local.com", Name = "Mahmoud Tahrawi", Role = Role.Customer };
				customer.PasswordHash = hasher.HashPassword(customer, "123");
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
