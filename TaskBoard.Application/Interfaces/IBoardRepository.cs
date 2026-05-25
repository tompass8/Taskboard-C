using TaskBoard.Domain.Entities;

namespace TaskBoard.Application.Interfaces;

public interface IBoardRepository
{
    Task<Board?> GetByIDAsync(int ID);
    Task<IEnumerable<Board>> GetBoardsByWorkspaceIDAsync(int workspaceID);
    
    void Add(Board board);
    void Update(Board board);
    void Delete(Board board);
    Task SaveChangesAsync();

}