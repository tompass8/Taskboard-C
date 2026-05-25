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