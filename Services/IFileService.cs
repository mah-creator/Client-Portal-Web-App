using ClientPortalApi.Models;

namespace ClientPortalApi.Services
{
    public interface IFileService
    {
        Task<FileEntity> SaveFileAsync(IFormFile file, string? projectId = null, string? taskId = null, string uploaderId = "");
    }
}
