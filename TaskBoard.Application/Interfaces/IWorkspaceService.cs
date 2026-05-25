using TaskBoard.Application.DTOs.Workspaces;

namespace TaskBoard.Application.Interfaces;

public interface IWorkspaceService
{
    Task<IEnumerable<WorkspaceDto>> GetUserWorkspacesAsync(Guid userId);
    Task<WorkspaceDto> CreateWorkspaceAsync(CreateWorkspaceRequest request, Guid userId);
    Task<IEnumerable<WorkspaceMembershipDto>> GetWorkspaceMembersAsync(int workspaceId, Guid requesterId);
    Task<WorkspaceMembershipDto> AddWorkspaceMemberAsync(int workspaceId, CreateWorkspaceMembershipRequest request, Guid requesterId);
    Task<WorkspaceMembershipDto> UpdateWorkspaceMemberRoleAsync(int workspaceId, Guid memberUserId, UpdateWorkspaceMembershipRoleRequest request, Guid requesterId);
    Task RemoveWorkspaceMemberAsync(int workspaceId, Guid memberUserId, Guid requesterId);
}