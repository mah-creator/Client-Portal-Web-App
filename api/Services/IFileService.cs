using ClientPortalApi.Models;

namespace ClientPortalApi.Services
{
    public interface IFileService
    {
        Task<FileEntity> SaveFileAsync(IFormFile file, string? projectId = null, string? taskId = null, string uploaderId = "");

        Task<FileAttr?> GetFile(int id);

	}

	public struct FileAttr
	{
		public string FileName { get; set; }
		public string FilePath { get; set; }
		public string ContentType { get; set; }
	}

}
