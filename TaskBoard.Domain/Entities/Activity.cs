namespace TaskBoard.Domain.Entities;

public class Activity
{
    public int ID { get; set; }
    public string Notification { get; set; } = string.Empty;
    public string? Comment { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    
    // Relations Foreign Keys
    public int CardID { get; set; }
    public Guid UserID { get; set; }
    
    // Null-forgiving operator (null!) : tells the compiler that EF Core
    // will populate this property at runtime, suppressing warning CS8618.
    public Card Card { get; set; } = null!;
    public User User { get; set; } = null!;
}