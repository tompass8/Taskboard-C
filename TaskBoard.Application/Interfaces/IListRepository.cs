using TaskBoard.Domain.Entities;

namespace TaskBoard.Application.Interfaces;

public interface IListRepository
{
    Task<List?> GetByIDAsync(int ID);
    Task<IEnumerable<List>> GetListsByBoardIDAsync(int boardID);
    void Update(List list);
    void Add(List list);
    Task SaveChangesAsync();
}