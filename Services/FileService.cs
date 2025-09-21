using ClientPortalApi.Data;
using ClientPortalApi.Models;

namespace ClientPortalApi.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _env;
        private readonly AppDbContext _db;
        public FileService(IWebHostEnvironment env, AppDbContext db) { _env = env; _db = db; }

        public async Task<FileEntity> SaveFileAsync(IFormFile file, string? projectId = null, string? taskId = null, string uploaderId = "")
        {
            var uploads = Path.Combine(_env.WebRootPath ?? "wwwroot", "uploads");
            if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);

            var safeName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
            var full = Path.Combine(uploads, safeName);
            using (var stream = File.Create(full)) {
                await file.CopyToAsync(stream);
            }

            var entity = new FileEntity
            {
                UploaderId = uploaderId,
                ProjectId = projectId,
                TaskId = taskId,
                Filename = file.FileName,
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
