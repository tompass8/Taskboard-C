using TaskBoard.Application.DTOs;
using TaskBoard.Application.DTOs.Boards;

namespace TaskBoard.Application.Interfaces;

public interface IBoardService
{
    Task<BoardResponse?> GetBoardByIDAsync(int ID);
    Task<IEnumerable<BoardDto>> GetBoardsByWorkspaceIDAsync(int workspaceID);
    Task<BoardDto> CreateBoardAsync(CreateBoardRequest request, Guid userID);
    Task<BoardResponse?> UpdateBoardAsync(int ID, UpdateBoardRequest request);
    Task DeleteBoardAsync(int ID);
}