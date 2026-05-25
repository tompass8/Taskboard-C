namespace TaskBoard.Domain.Entities;

public class Workspace
{
    public int ID { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
  
    // ICollection<T> : interface representing a navigable collection of related entities.
        // Used instead of List<T> to stay independent of the concrete implementation,
        // allowing EF Core to substitute its own optimized proxy at runtime.
        // Initialized with new List<T>() to avoid null reference exceptions
        // before EF Core loads the data.
    public ICollection<WorkspaceMembership> WorkspaceMemberships { get; set; } = new List<WorkspaceMembership>();
    public ICollection<Board> Boards { get; set; } = new List<Board>();
    
} 
