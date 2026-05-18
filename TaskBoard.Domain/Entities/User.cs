using System.Net.Mime;

namespace TaskBoard.Domain.Entities;

public class User
{
    public Guid ID { get; set; } = Guid.NewGuid();
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty; // BCrypt used
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // ICollection<T> : interface representing a navigable collection of related entities.
        // Used instead of List<T> to stay independent of the concrete implementation,
        // allowing EF Core to substitute its own optimized proxy at runtime.
        // Initialized with new List<T>() to avoid null reference exceptions
        // before EF Core loads the data.
    public ICollection<WorkspaceMembership> WorkspaceMemberships { get; set; } = new List<WorkspaceMembership>();
    public ICollection<BoardMembership> BoardMemberships { get; set; } = new List<BoardMembership>();
    public ICollection<Card> AssignedCards { get; set; } = new List<Card>();
    public ICollection<Activity> Activities { get; set; } = new List<Activity>();
}