using ClientPortalApi.Data;
using Microsoft.AspNetCore.SignalR;
using ClientPortalApi.Hubs;
using Microsoft.EntityFrameworkCore;

namespace ClientPortalApi.Services
{
    public class NotificationService : INotificationService
    {
        private readonly AppDbContext _db;
        private readonly IHubContext<NotificationsHub> _hub;
        public NotificationService(AppDbContext db, IHubContext<NotificationsHub> hub) { _db = db; _hub = hub; }

        public async Task NotifyProjectMembersAsync(string projectId, string message)
        {
            var members = await _db.ProjectMembers.Where(pm => pm.ProjectId == projectId).ToListAsync();
            foreach (var m in members) {
                await _hub.Clients.Group(m.UserId).SendAsync("ReceiveNotification", new { projectId, message, createdAt = DateTime.UtcNow });
            }
        }
    }
}
