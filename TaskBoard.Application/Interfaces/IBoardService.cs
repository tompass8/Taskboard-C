using TaskBoard.Application.DTOs;

namespace TaskBoard.Application.Interfaces;

public interface IBoardService
{
    Task<IEnumerable<BoardResponse>> GetBoardsByWorkspaceAsync(int workspaceId);
    Task<BoardResponse> CreateBoardAsync(CreateBoardRequest request);
    Task<BoardResponse> UpdateBoardAsync(int id, UpdateBoardRequest request);
    Task DeleteBoardAsync(int id);
}