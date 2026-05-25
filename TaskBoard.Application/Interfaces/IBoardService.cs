using TaskBoard.Application.DTOs.Boards;

namespace TaskBoard.Application.Interfaces;

public interface IBoardService
{
    Task<IEnumerable<BoardDto>> GetBoardsByWorkspaceIDAsync(int workspaceID);
    Task<BoardDto> CreateBoardAsync(CreateBoardRequest request, Guid userId);
    Task<BoardDto?> UpdateBoardAsync(int ID, UpdateBoardRequest request);
    Task DeleteBoardAsync(int ID);
}
