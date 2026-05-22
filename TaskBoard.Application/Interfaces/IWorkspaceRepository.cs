using TaskBoard.Domain.Entities;

namespace TaskBoard.Application.Interfaces;

public interface IWorkspaceRepository
{
    Task<IEnumerable<Workspace>> GetUserWorkspacesAsync(int userId); // Guid remplacé par int
    Task AddAsync(Workspace workspace);
    Task SaveChangesAsync();
}