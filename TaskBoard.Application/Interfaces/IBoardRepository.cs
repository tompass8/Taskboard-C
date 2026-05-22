using TaskBoard.Domain.Entities;

namespace TaskBoard.Application.Interfaces;

public interface IBoardRepository
{
    Task<Board?> GetByIdAsync(int id);
    Task<IEnumerable<Board>> GetByWorkspaceIdAsync(int workspaceID);
    void Add(Board board);
    void Update(Board board);
    void Delete(Board board);
    Task SaveChangesAsync();
}