using TaskBoard.Domain.Entities;

namespace TaskBoard.Application.Interfaces;

public interface IListRepository
{
    Task<List?> GetByIdAsync(int ID);
    Task<IEnumerable<List>> GetListsByBoardIDAsync(int boardID);
    
    void Update(List list);
    Task AddAsync(List list);
    
    Task SaveChangesAsync();
}