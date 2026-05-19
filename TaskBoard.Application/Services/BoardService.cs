using TaskBoard.Application.DTOs.Boards;
using TaskBoard.Application.Interfaces;
using TaskBoard.Domain.Entities;

namespace TaskBoard.Application.Services;

public class BoardService : IBoardService
{
    private readonly IBoardRepository _boardRepository;

    public BoardService(IBoardRepository boardRepository)
    {
        _boardRepository = boardRepository;
    }

    public async Task<IEnumerable<BoardDto>> GetBoardsByWorkspaceIdAsync(int workspaceId)
    {
        var boards = await _boardRepository.GetBoardsByWorkspaceIdAsync(workspaceId);
        
        return boards.Select(b => new BoardDto
        {
            ID = b.ID,
            Title = b.Title, // Remplacé
            Description = b.Description,
            WorkspaceID = b.WorkspaceID, // Remplacé
            CreatedAt = b.CreatedAt
        });
    }

    public async Task<BoardDto> CreateBoardAsync(CreateBoardRequest request, Guid userId)
    {
        var board = new Board
        {
            Title = request.Title, // Remplacé
            Description = request.Description,
            WorkspaceID = request.WorkspaceID, // Remplacé
            OwnerID = userId // Remplacé par le Guid
        };

        await _boardRepository.AddAsync(board);
        await _boardRepository.SaveChangesAsync();

        return new BoardDto
        {
            ID = board.ID,
            Title = board.Title,
            Description = board.Description,
            WorkspaceID = board.WorkspaceID,
            CreatedAt = board.CreatedAt
        };
    }
}