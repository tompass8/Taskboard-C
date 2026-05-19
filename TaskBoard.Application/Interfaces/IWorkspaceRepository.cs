using TaskBoard.Domain.Entities;

namespace TaskBoard.Application.Interfaces;

public interface IWorkspaceRepository
{
    Task<IEnumerable<Workspace>> GetWorkspacesByUserIdAsync(Guid userId);
    Task<Workspace?> GetByIdAsync(Guid id);
    Task AddAsync(Workspace workspace);
    Task SaveChangesAsync();
}