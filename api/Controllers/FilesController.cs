using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ClientPortalApi.Services;
using System.Security.Claims;
using ClientPortalApi.Data;
using Microsoft.EntityFrameworkCore;
using ClientPortalApi.DTOs;

namespace ClientPortalApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FilesController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly AppDbContext _db;
        public FilesController(IFileService fileService, AppDbContext db) { _fileService = fileService; _db = db; }

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

            return Ok(new FileResponse(entity.Filename, projectTitle!, entity.Size, uploaderName!, entity.UploadedAt, entity.Path));
        }

        [HttpGet("task/{Id}")]
        [ProducesResponseType(typeof(FileResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTaskFiles(string Id)
        {
            if (!_db.TaskItems.Any(t => t.Id == Id)) return BadRequest("Task wasn't found");

            var files = _db.Files.Include(f => f.Uploader).Include(f => f.Project)
                .Where(f => f.TaskId == Id).Select(f => new FileResponse(f.Filename, f.Project == null ? null : f.Project.Title, f.Size, f.Uploader == null ? null : f.Uploader.Name, f.UploadedAt, f.Path));

            return Ok(files);
        }

        [HttpGet("project/{Id}")]
        [ProducesResponseType(typeof(FileResponse), StatusCodes.Status200OK)]
		public async Task<IActionResult> GetProjectFiles(string Id)
        {
			if (!_db.Projects.Any(p => p.Id == Id)) return BadRequest("Project wasn't found");

			var files = _db.Files.Include(f => f.Uploader).Include(f => f.Project)
                .Where(f => f.ProjectId == Id).Select(f => new FileResponse(f.Filename, f.Project == null ? null : f.Project.Title, f.Size, f.Uploader == null ? null : f.Uploader.Name, f.UploadedAt, f.Path));

            return Ok(files);
        }

        [HttpGet("recent")]
        [ProducesResponseType(typeof(FileResponse), StatusCodes.Status200OK)]
		public async Task<IActionResult> GetRecentFiles()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized("");

            var files = _db.ProjectMembers
                .Where(mem => mem.UserId == userId)
                .Join(_db.Files, mem => mem.ProjectId, f => f.ProjectId, (mem, f) => f)
                .AsEnumerable().Where(f => f.UploadedAt.Add(TimeSpan.FromHours(2)) > DateTime.UtcNow)
                .Select(f => new FileResponse(
                    f.Filename,
                    f.Project == null ? null : f.Project.Title, f.Size,
                    f.Uploader == null ? null : f.Uploader.Name,
                    f.UploadedAt, f.Path));

            return Ok(files);
        }
    }
}
