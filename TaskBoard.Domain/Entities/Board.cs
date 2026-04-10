namespace TaskBoard.Domain.Entities;

public class Board
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Relations
    public Guid WorkspaceId { get; set; }
    public Workspace Workspace { get; set; } = null!;

    public ICollection<List> Lists { get; set; } = new List<List>();
    public ICollection<Label> Labels { get; set; } = new List<Label>();
}