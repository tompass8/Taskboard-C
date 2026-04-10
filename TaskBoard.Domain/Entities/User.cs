namespace TaskBoard.Domain.Entities;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Relations
    public ICollection<Workspace> OwnedWorkspaces { get; set; } = new List<Workspace>();
    public ICollection<Workspace> Workspaces { get; set; } = new List<Workspace>(); // N-N Membre
    public ICollection<Card> AssignedCards { get; set; } = new List<Card>(); // N-N Assignation
}