using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ClientPortalApi.Services;
using System.Security.Claims;
using ClientPortalApi.Data;
using Microsoft.EntityFrameworkCore;
using ClientPortalApi.DTOs;
using System.Net.Http.Headers;
using System.Net;
using Microsoft.AspNetCore.StaticFiles;
using ClientPortalApi.Paging;
using ClientPortalApi.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ClientPortalApi.Services.Notifications;

namespace ClientPortalApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FilesController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly AppDbContext _db;
		private readonly FileExtensionContentTypeProvider _ext;
		private readonly INotificationHubService _notify;

		public FilesController(IFileService fileService, AppDbContext db, FileExtensionContentTypeProvider ext, INotificationHubService notify) { _fileService = fileService; _db = db; _ext = ext; _notify = notify; }

        [HttpPost("upload")]
        [ProducesResponseType(typeof(FileResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Upload([FromForm] IFormFile file, [FromForm] string? projectId, [FromForm] string? taskId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (file == null) return BadRequest("File required");
            if (!_db.Projects.Any(p => p.Id == projectId)) return BadRequest("Project wasn't found");
            if (!_db.TaskItems.Any(t => t.Id == taskId)) return BadRequest("Task wasn't found");

            var entity = await _fileService.SaveFileAsync(file, projectId, taskId, userId);
            var projectTitle = _db.Projects.Find(projectId)?.Title;
            var uploaderName = _db.Users.Find(entity.UploaderId)?.Name;

            await _notify.SendNotificationToUsers(_db.ProjectMembers.Where(p => p.ProjectId == projectId)
                .Select(p => p.UserId).Except([userId]),
                new NotificationDto
                {
                    Title = "File Uploaded",
                    Message = $"{uploaderName} uploaded a file '{entity.Filename}' to project '{projectTitle}'",
                    Type = NotificationType.Info
                });

            return Ok(new FileResponse(entity.Id, taskId!, entity.Filename, projectTitle!, entity.Size, uploaderName!, entity.UploadedAt, entity.Path, file.ContentType));
        }

        [HttpGet("task/{Id}")]
        [ProducesResponseType(typeof(PagedList<FileResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTaskFiles(string Id, int? page, int? pageSize)
        {
            if (!_db.TaskItems.Any(t => t.Id == Id)) return BadRequest("Task wasn't found");
            var files = _db.Files.Include(f => f.Uploader).Include(f => f.Project)
                .Where(f => f.TaskId == Id).OrderByDescending(f => f.UploadedAt).AsEnumerable().Select(f =>
                {
                    var contentType = getContentType(f.Path);
					return new FileResponse(f.Id, f.TaskId!, f.Filename, f.Project?.Title!, f.Size, f.Uploader?.Name!, f.UploadedAt, f.Path, contentType);
                }).AsQueryable();

			return Ok(PagedList<FileResponse>.CreatePagedList(files, page, pageSize));
		}

		private string getContentType(string path)
		{
			_ext.TryGetContentType(path, out string? contentType);

            return contentType ?? "application/octet-stream";
		}

		[HttpGet("project/{Id}")]
        [ProducesResponseType(typeof(PagedList<FileResponse>), StatusCodes.Status200OK)]
		public async Task<IActionResult> GetProjectFiles(string Id, int? page, int? pageSize)
        {
			if (!_db.Projects.Any(p => p.Id == Id)) return BadRequest("Project wasn't found");

			var files = _db.Files.Include(f => f.Uploader).Include(f => f.Project)
                .Where(f => f.ProjectId == Id).OrderByDescending(f => f.UploadedAt).AsEnumerable().Select(f =>
				{
					var contentType = getContentType(f.Path);
					return new FileResponse(f.Id, f.TaskId!, f.Filename, f.Project?.Title!, f.Size, f.Uploader?.Name!, f.UploadedAt, f.Path, contentType);
				}).AsQueryable();

			return Ok(PagedList<FileResponse>.CreatePagedList(files, page, pageSize));
        }

        [HttpGet("recent")]
        [ProducesResponseType(typeof(PagedList<FileResponse>), StatusCodes.Status200OK)]
		public async Task<IActionResult> GetRecentFiles(int? page, int? pageSize)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized("");

            // TODO: make recent timespan configurable

            var files = _db.ProjectMembers
                .Where(mem => mem.UserId == userId)
                .Join(_db.Files, mem => mem.ProjectId, f => f.ProjectId, (mem, f) => f)
                .AsEnumerable().Where(f => f.UploadedAt.Add(TimeSpan.FromHours(2)) > DateTime.UtcNow)
				.OrderByDescending(f => f.UploadedAt)
                .Select(f =>
				{
					var contentType = getContentType(f.Path);
					return new FileResponse(f.Id, f.TaskId!, f.Filename, f.Project?.Title!, f.Size, f.Uploader?.Name!, f.UploadedAt, f.Path, contentType);
				}).AsQueryable();

			return Ok(PagedList<FileResponse>.CreatePagedList(files, page, pageSize));
		}

		[HttpGet("{id}")]
        [AllowAnonymous]
		public async Task<ActionResult> GetFile(int id)
		{
			var file = await _fileService.GetFile(id);
			if (file is null) 
                return NotFound("File not found");

            var fileBytes = await System.IO.File.ReadAllBytesAsync(file?.FilePath!);

			// Add the "Access-Control-Expose-Headers" header
			Response.Headers.TryAdd("Content-Disposition", "attachment");

			return File(fileBytes, file?.ContentType!, file?.FileName);
		}
	}
}
