namespace TaskBoard.Domain.Entities;

public class Workspace
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Relations
    public Guid OwnerId { get; set; }
    public User Owner { get; set; } = null!;
    
    public ICollection<User> Members { get; set; } = new List<User>();
    public ICollection<Board> Boards { get; set; } = new List<Board>();
}