using TaskBoard.Application.DTOs;

namespace TaskBoard.Application.Interfaces;

public interface IBoardService
{
    Task<IEnumerable<BoardResponse>> GetBoardsByWorkspaceAsync(int workspaceID);
    Task<BoardResponse> CreateBoardAsync(CreateBoardRequest request);
    Task<BoardResponse?> UpdateBoardAsync(int ID, UpdateBoardRequest request);
    Task DeleteBoardAsync(int ID);
}