using TaskBoard.Application.DTOs;
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
    
    public async Task<IEnumerable<BoardResponse>> GetBoardsByWorkspaceAsync(int workspaceId)
    {
        var boards = await _boardRepository.GetByWorkspaceIdAsync(workspaceId);
    
        return boards.Select(b => new BoardResponse(b.ID, b.Title, b.Description, b.WorkspaceID));
    }

    public async Task<BoardResponse> CreateBoardAsync(CreateBoardRequest request)
    {
        var board = new Board
        {
            Title = request.Title,
            Description = request.Description,
            WorkspaceID = request.WorkspaceID
        };

        _boardRepository.Add(board);
        await _boardRepository.SaveChangesAsync();

        return new BoardResponse(board.ID, board.Title, board.Description, board.WorkspaceID);
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