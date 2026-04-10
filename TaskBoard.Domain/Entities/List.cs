namespace TaskBoard.Domain.Entities;

public class List
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public int Position { get; set; } // Crucial pour le Drag & Drop

    // Relations
    public Guid BoardId { get; set; }
    public Board Board { get; set; } = null!;

    public ICollection<Card> Cards { get; set; } = new List<Card>();
}