using TaskBoard.Application.DTOs.Boards;

namespace TaskBoard.Application.Interfaces;

public interface IBoardService
{
    Task<IEnumerable<BoardDto>> GetBoardsByWorkspaceIDAsync(int workspaceID);
    Task<BoardDto> CreateBoardAsync(CreateBoardRequest request, Guid userID);
    
    Task<BoardResponse> CreateBoardAsync(CreateBoardRequest request);
    Task<BoardResponse?> UpdateBoardAsync(int ID, UpdateBoardRequest request);
    Task DeleteBoardAsync(int ID);
}