namespace TaskBoard.Domain.Entities;

public class WorkspaceMembership
{
    public int ID { get; set; }
    public Role Role { get; set; } = Role.User;
    
    // Relation Foreign Keys
    public int WorkspaceID { get; set; }
    public Guid UserID { get; set; }    
    
    // Null-forgiving operator (null!) : tells the compiler that EF Core
        // will populate this property at runtime, suppressing warning CS8618.
    public Workspace Workspace { get; set; } = null!;
    public User User { get; set; } = null!;
}