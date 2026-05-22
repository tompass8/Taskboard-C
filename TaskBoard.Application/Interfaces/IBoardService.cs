using TaskBoard.Application.DTOs.Boards;

namespace TaskBoard.Application.Interfaces;

public interface IBoardService
{
    Task<IEnumerable<BoardDto>> GetBoardsByWorkspaceIdAsync(int workspaceId);
    
    // Attention ici : on repasse le userId en Guid pour coller à OwnerID !
    Task<BoardDto> CreateBoardAsync(CreateBoardRequest request, Guid userId);
}