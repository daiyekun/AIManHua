namespace AIManHua.Domain.Entities;

public class ComicTask
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Prompt { get; set; } = string.Empty;
    public string Style { get; set; } = "manga";
    public string Status { get; set; } = "pending";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }
    public User User { get; set; } = null!;
    public ICollection<Storyboard> Storyboards { get; set; } = new List<Storyboard>();
    public ICollection<GeneratedImage> Images { get; set; } = new List<GeneratedImage>();
}
