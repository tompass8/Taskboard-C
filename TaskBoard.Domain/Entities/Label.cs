namespace TaskBoard.Domain.Entities;

public class Label
{
    public int ID { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ColorHex { get; set; } = "#cccccc";

    // Relation Foreign Keys
    public int CardID { get; set; }
    
    // Null-forgiving operator (null!) : tells the compiler that EF Core
        // will populate this property at runtime, suppressing warning CS8618.
    public Card Card { get; set; } = null!;
}