namespace TaskBoard.Domain.Entities;

public class List
{
    public int ID { get; set; }
    public string Title { get; set; } = string.Empty;
    public int Position { get; set; } = 0;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    
    // Relations Foreign Keys
    public int BoardID { get; set; }
    
    // Null-forgiving operator (null!) : tells the compiler that EF Core
        // will populate this property at runtime, suppressing warning CS8618.
    public Board Board { get; set; } = null!; 
    
    // ICollection<T> : interface representing a navigable collection of related entities.
        // Used instead of List<T> to stay independent of the concrete implementation,
        // allowing EF Core to substitute its own optimized proxy at runtime.
        // Initialized with new List<T>() to avoid null reference exceptions
        // before EF Core loads the data.
    public ICollection<Card> Cards { get; set; } = new List<Card>();
}