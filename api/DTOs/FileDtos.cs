namespace ClientPortalApi.DTOs;

public record FileResponse(string Filename, string ProjectTitle, long Size, string Uploader, DateTime UploadedAt, string Path);