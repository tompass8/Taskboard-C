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

    public async Task<IEnumerable<BoardDto>> GetBoardsByWorkspaceIDAsync(int workspaceID)
    {
        var boards = await _boardRepository.GetBoardsByWorkspaceIDAsync(workspaceID);
        return boards.Select(b => new BoardDto
        {
            ID = b.ID,
            Title = b.Title,
            Description = b.Description,
            WorkspaceID = b.WorkspaceID,
            CreatedAt = b.CreatedAt
        });
    }

    public async Task<BoardDto> CreateBoardAsync(CreateBoardRequest request, Guid userId)
    {
        var board = new Board
        {
            Title = request.Title,
            Description = request.Description,
            WorkspaceID = request.WorkspaceID,
            OwnerID = userID
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
    
    public async Task<BoardResponse?> UpdateBoardAsync(int id, UpdateBoardRequest request)
    {
        var board = await _boardRepository.GetByIdAsync(id);
        if (board == null) return null;

        if (request.Title != null) board.Title = request.Title;
        if (request.Description != null) board.Description = request.Description;

        _boardRepository.Update(board);
        await _boardRepository.SaveChangesAsync();

        return new BoardResponse(board.ID, board.Title, board.Description, board.WorkspaceID);
    }

 
    public async Task DeleteBoardAsync(int id)
    {
        var board = await _boardRepository.GetByIdAsync(id);
        if (board == null) return;

        _boardRepository.Delete(board);
        await _boardRepository.SaveChangesAsync();
    }
}