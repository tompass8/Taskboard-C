namespace TaskBoard.Domain.Entities;

public class Card
{
    public int ID { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Position { get; set; } = 0;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public DateTime? Deadline { get; set; }
    
    // Relations Foreign Keys
    public int ListID { get; set; }
    public Guid? AssignedUserID { get; set; }
    
    public User? AssignedUser { get; set; }
    
    // Null-forgiving operator (null!) : tells the compiler that EF Core
        // will populate this property at runtime, suppressing warning CS8618.
    public List List { get; set; } = null!;
    
    // ICollection<T> : interface representing a navigable collection of related entities.
        // Used instead of List<T> to stay independent of the concrete implementation,
        // allowing EF Core to substitute its own optimized proxy at runtime.
        // Initialized with new List<T>() to avoid null reference exceptions
        // before EF Core loads the data.
    public ICollection<Label> Labels { get; set; } = new List<Label>();
    public ICollection<Activity> Activities { get; set; } = new List<Activity>();
    
    
    
}