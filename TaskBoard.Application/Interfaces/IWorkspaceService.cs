using TaskBoard.Application.DTOs.Workspaces;

namespace TaskBoard.Application.Interfaces;

public interface IWorkspaceService
{
    Task<IEnumerable<WorkspaceDto>> GetUserWorkspacesAsync(Guid userId);
    Task<WorkspaceDto> CreateWorkspaceAsync(CreateWorkspaceRequest request, Guid userId);
}