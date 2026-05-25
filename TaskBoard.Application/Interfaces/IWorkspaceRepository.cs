using TaskBoard.Domain.Entities;

namespace TaskBoard.Application.Interfaces;

public interface IWorkspaceRepository
{
    Task<IEnumerable<Workspace>> GetUserWorkspacesAsync(Guid userId);
    Task AddAsync(Workspace workspace);
    Task SaveChangesAsync();
}