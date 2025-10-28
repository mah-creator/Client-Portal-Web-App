using Microsoft.EntityFrameworkCore;
using ClientPortalApi.Models;

namespace ClientPortalApi.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Project> Projects => Set<Project>();
        public DbSet<ProjectInvitation> Invitations => Set<ProjectInvitation>();
		public DbSet<ProjectMember> ProjectMembers => Set<ProjectMember>();
        public DbSet<TaskItem> TaskItems => Set<TaskItem>();
        public DbSet<FileEntity> Files => Set<FileEntity>();
        public DbSet<Comment> Comments => Set<Comment>();
		public DbSet<Notification> Notifications => Set<Notification>();

		public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts) { }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			base.OnConfiguring(optionsBuilder);

            optionsBuilder.EnableSensitiveDataLogging();
		}

		protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<User>().HasKey(u => u.Id);
            builder.Entity<Profile>().HasKey(p => p.Id);
            builder.Entity<Project>().HasKey(p => p.Id);
            builder.Entity<TaskItem>().HasKey(t => t.Id);
            builder.Entity<ProjectMember>().HasKey(pm => new { pm.ProjectId, pm.UserId });
            builder.Entity<ProjectInvitation>().HasKey(pi => pi.Id);
            builder.Entity<Notification>().HasKey(n => n.Id);

			builder.Entity<User>().HasOne(u => u.Profile).WithOne().HasForeignKey<Profile>(p => p.Id).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Comment>().HasOne(c => c.User).WithMany().HasForeignKey(c => c.UserId);
            builder.Entity<FileEntity>().HasOne(f => f.Uploader).WithMany().HasForeignKey(f => f.UploaderId);
            builder.Entity<FileEntity>().HasOne(f => f.Project).WithMany().HasForeignKey(f => f.ProjectId);
            builder.Entity<User>().HasMany(u => u.Invitations).WithOne(i => i.Invitee)
                .HasForeignKey(i => i.InviteeId).OnDelete(DeleteBehavior.Cascade);
			builder.Entity<User>().HasMany<ProjectInvitation>().WithOne(i => i.Inviter)
				.HasForeignKey(i => i.InviterId).OnDelete(DeleteBehavior.Cascade);
			builder.Entity<User>().HasMany(u => u.Notifications).WithOne()
				.HasForeignKey(n => n.UserId).OnDelete(DeleteBehavior.Cascade);
			builder.Entity<Project>().HasMany<ProjectInvitation>().WithOne()
				.HasForeignKey(i => i.ProjectId).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<ProjectInvitation>().HasOne(i => i.Project).WithMany()
                .HasForeignKey(i => i.ProjectId);
            builder.Entity<ProjectMember>().HasOne(m => m.Project).WithMany(p => p.Members)
				.HasForeignKey(m => m.ProjectId).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<ProjectMember>().HasOne(m => m.User).WithMany(u => u.Projects)
                .HasForeignKey(m => m.UserId).OnDelete(DeleteBehavior.Cascade);

			builder.Entity<User>().Navigation(u  => u.Profile).IsRequired();
            builder.Entity<ProjectInvitation>().Navigation(i => i.Invitee).IsRequired();
			builder.Entity<User>().Property(u => u.Id).ValueGeneratedNever();
			builder.Entity<Project>().Property(p => p.Id).ValueGeneratedNever();
            builder.Entity<TaskItem>().Property(t => t.Id).ValueGeneratedNever();
            builder.Entity<Comment>().Property(c => c.Id).ValueGeneratedNever();
            builder.Entity<Notification>().OwnsOne(n => n.Metadata);
		}
    }
}
