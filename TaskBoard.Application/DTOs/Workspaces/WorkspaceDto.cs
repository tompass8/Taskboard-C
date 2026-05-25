using TaskBoard.Domain.Entities;

namespace TaskBoard.Application.DTOs.Workspaces;

public class WorkspaceDto
{
    public int ID { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}

public class CreateWorkspaceRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}

public class WorkspaceMembershipDto
{
    public int ID { get; set; }
    public int WorkspaceID { get; set; }
    public Guid UserID { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Role Role { get; set; }
}

public class CreateWorkspaceMembershipRequest
{
    public Guid UserID { get; set; }
    public Role Role { get; set; }
}

public class UpdateWorkspaceMembershipRoleRequest
{
    public Role Role { get; set; }
}