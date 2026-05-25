using TaskBoard.Domain.Entities;

namespace TaskBoard.Application.Interfaces;

public interface IWorkspaceRepository
{
    Task<IEnumerable<Workspace>> GetUserWorkspacesAsync(Guid userId);
    Task<Workspace?> GetByIdAsync(int workspaceId);
    Task<WorkspaceMembership?> GetWorkspaceMembershipAsync(int workspaceId, Guid userId);
    Task<IEnumerable<WorkspaceMembership>> GetWorkspaceMembersAsync(int workspaceId);
    Task AddAsync(Workspace workspace);
    Task AddWorkspaceMembershipAsync(WorkspaceMembership membership);
    Task RemoveWorkspaceMembershipAsync(WorkspaceMembership membership);
    Task SaveChangesAsync();
}