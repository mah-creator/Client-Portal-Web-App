namespace ClientPortalApi.Services
{
    public interface INotificationService
    {
        Task NotifyProjectMembersAsync(string projectId, string message);
    }
}
