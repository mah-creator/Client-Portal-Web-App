namespace ClientPortalApi.DTOs;

public record FileResponse(int Id, string taskId, string Filename, string ProjectTitle, long Size, string Uploader, DateTime UploadedAt, string Path, string ContentType);