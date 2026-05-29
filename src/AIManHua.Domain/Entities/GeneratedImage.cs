namespace AIManHua.Domain.Entities;

public class GeneratedImage
{
    public long Id { get; set; }
    public long ComicTaskId { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string MinioObjectKey { get; set; } = string.Empty;
    public int Width { get; set; }
    public int Height { get; set; }
    public long FileSizeBytes { get; set; }
    public string ContentType { get; set; } = "image/png";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ComicTask ComicTask { get; set; } = null!;
}
