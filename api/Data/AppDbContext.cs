using Microsoft.EntityFrameworkCore;
using ClientPortalApi.Models;

namespace ClientPortalApi.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Project> Projects => Set<Project>();
        public DbSet<ProjectMember> ProjectMembers => Set<ProjectMember>();
        public DbSet<TaskItem> TaskItems => Set<TaskItem>();
        public DbSet<FileEntity> Files => Set<FileEntity>();
        public DbSet<Comment> Comments => Set<Comment>();

        public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<User>().HasKey(u => u.Id);
            builder.Entity<Project>().HasKey(p => p.Id);
            builder.Entity<TaskItem>().HasKey(t => t.Id);
            builder.Entity<ProjectMember>().HasKey(pm => new { pm.ProjectId, pm.UserId });

            builder.Entity<Comment>().HasOne(c => c.User).WithMany().HasForeignKey(c => c.UserId);
            builder.Entity<FileEntity>().HasOne(f => f.Uploader).WithMany().HasForeignKey(f => f.UploaderId);
            builder.Entity<FileEntity>().HasOne(f => f.Project).WithMany().HasForeignKey(f => f.ProjectId);

			builder.Entity<User>().Property(u => u.Id).ValueGeneratedNever();
            builder.Entity<Project>().Property(p => p.Id).ValueGeneratedNever();
            builder.Entity<TaskItem>().Property(t => t.Id).ValueGeneratedNever();
            builder.Entity<Comment>().Property(c => c.Id).ValueGeneratedNever();
        }
    }
}
