namespace TaskBoard.Domain.Entities;

public class Card
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
    public int Position { get; set; } // Crucial pour l'ordre dans la colonne
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Relations
    public Guid ListId { get; set; }
    public List List { get; set; } = null!;

    public ICollection<User> Assignees { get; set; } = new List<User>();
    public ICollection<Label> Labels { get; set; } = new List<Label>();
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
}