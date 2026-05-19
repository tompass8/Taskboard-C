using TaskBoard.Application.DTOs.Workspaces;

namespace TaskBoard.Application.Interfaces;

public interface IWorkspaceService
{
    Task<IEnumerable<WorkspaceDto>> GetUserWorkspacesAsync(int userId); // Guid remplacé par int
    Task<WorkspaceDto> CreateWorkspaceAsync(CreateWorkspaceRequest request, int userId); // Guid remplacé par int
}