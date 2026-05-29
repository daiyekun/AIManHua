namespace AIManHua.Domain.Entities;

public class Storyboard
{
    public long Id { get; set; }
    public long ComicTaskId { get; set; }
    public int PanelIndex { get; set; }
    public string SceneDescription { get; set; } = string.Empty;
    public string Dialogue { get; set; } = string.Empty;
    public string LayoutType { get; set; } = "full";
    public int X { get; set; }
    public int Y { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ComicTask ComicTask { get; set; } = null!;
}
