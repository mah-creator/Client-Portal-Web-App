namespace ClientPortalApi.Models
{
    public class FileEntity
    {
        public int Id { get; set; }
        public string UploaderId { get; set; } = null!;
        public User Uploader { get; set; } = null!;
        public string? ProjectId { get; set; }
        public Project? Project { get; set; }
        public string? TaskId { get; set; }
        public string Filename { get; set; } = null!;
        public string Path { get; set; } = null!;
        public long Size { get; set; }
        public string? Mime { get; set; }
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }
}
