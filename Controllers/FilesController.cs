using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ClientPortalApi.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ClientPortalApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FilesController : ControllerBase
    {
        private readonly IFileService _fileService;
        public FilesController(IFileService fileService) { _fileService = fileService; }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] IFormFile file, [FromForm] string? projectId, [FromForm] string? taskId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (file == null) return BadRequest("File required");
            var entity = await _fileService.SaveFileAsync(file, projectId, taskId, userId);
            return Ok(entity);
        }
    }
}
