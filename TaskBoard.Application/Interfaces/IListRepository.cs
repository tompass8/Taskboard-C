using TaskBoard.Domain.Entities;

namespace TaskBoard.Application.Interfaces;

public interface IListRepository
{
    Task<List?> GetByIdAsync(int ID);
    
    Task<IEnumerable<List>> GetByBoardIdAsync(int boardID);
    
    void Update(List list);
    
    Task SaveChangesAsync();
}