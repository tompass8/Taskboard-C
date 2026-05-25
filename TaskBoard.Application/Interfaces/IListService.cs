using TaskBoard.Application.DTOs.Lists;

namespace TaskBoard.Application.Interfaces;

public interface IListService
{
    Task<IEnumerable<ListDto>> GetListsByBoardIDAsync(int boardID);
    
    Task<ListDto> CreateListAsync(CreateListRequest request, Guid userID);
    Task UpdateListPositionAsync(int listID, int newPosition);
}