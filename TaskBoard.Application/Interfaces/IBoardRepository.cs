using TaskBoard.Domain.Entities;

namespace TaskBoard.Application.Interfaces;

public interface IBoardRepository
{
    Task<Board?> GetByIdAsync(int ID);
    Task<IEnumerable<Board>> GetBoardsByWorkspaceIDAsync(int workspaceID);
    
    Task AddAsync(Board board);
    Task SaveChangesAsync();
    
    void Add(Board board);
    void Update(Board board);
    void Delete(Board board);


}