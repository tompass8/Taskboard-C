namespace TaskBoard.Domain.Entities;

public class Comment
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Guid CardId { get; set; }
    public Card Card { get; set; } = null!;

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}