namespace ClientPortalApi.DTOs;

public record FileResponse(int Id, string Filename, string ProjectTitle, long Size, string Uploader, DateTime UploadedAt, string Path);