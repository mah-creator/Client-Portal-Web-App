using ClientPortalApi.Data;
using ClientPortalApi.Models;
using Microsoft.AspNetCore.StaticFiles;

namespace ClientPortalApi.Services
{
	public class FileService : IFileService
	{
		private readonly IWebHostEnvironment _env;
		private readonly AppDbContext _db;
		public FileService(IWebHostEnvironment env, AppDbContext db) { _env = env; _db = db; }

		public async Task<FileAttr?> GetFile(int id)
		{
			var fileEntity = await _db.Files.FindAsync(id);

			if (fileEntity == null)
				return null;

			var filePath = Path.Combine(_env.WebRootPath, fileEntity.Path.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));

			//(new FileExtensionContentTypeProvider()).TryGetContentType(filePath, out string? contentType); // mime type of file

			return new FileAttr
			{
				FileName = fileEntity.Filename,
				FilePath = filePath,
				ContentType = "application/json" ?? "application/octet-stream"
			}; 
		}

		public async Task<FileEntity> SaveFileAsync(IFormFile file, string? projectId = null, string? taskId = null, string uploaderId = "")
		{
			var uploads = Path.Combine(_env.WebRootPath ?? "wwwroot", "uploads");
			if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);

			var fileName = Path.GetFileName(file.FileName); // file name without any invalid characters
			var safeName = $"{Guid.NewGuid()}_{fileName}";
			var full = Path.Combine(uploads, safeName);
			using (var stream = File.Create(full))
			{
				await file.CopyToAsync(stream);
			}

			var entity = new FileEntity
			{
				UploaderId = uploaderId,
				ProjectId = projectId,
				TaskId = taskId,
				Filename = fileName,
				Path = $"/uploads/{safeName}",
				Size = file.Length,
				Mime = file.ContentType,
			};
			_db.Files.Add(entity);
			await _db.SaveChangesAsync();
			return entity;
		}
	}
}
