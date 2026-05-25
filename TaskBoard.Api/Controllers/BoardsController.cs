using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskBoard.Api.Extensions;
using TaskBoard.Application.DTOs.Boards;
using TaskBoard.Application.Interfaces;
using CreateBoardRequest = TaskBoard.Application.DTOs.Boards.CreateBoardRequest;

namespace TaskBoard.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BoardsController : ControllerBase
{
    // Injecte le service applicatif à la place du DbContext
    private readonly IBoardService _boardService;

    public BoardsController(IBoardService boardService)
    {
        _boardService = boardService;
    }

    // Utilise une route spécifique pour récupérer les tableaux d'un espace précis
    [HttpGet("workspace/{workspaceID}")]
    public async Task<IActionResult> GetBoardsByWorkspace(int workspaceID)
    {
        var boards = await _boardService.GetBoardsByWorkspaceIDAsync(workspaceID);
        return Ok(boards);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBoard([FromBody] CreateBoardRequest request)
    {
        Guid userID = User.GetUserID(); 

        var createdBoard = await _boardService.CreateBoardAsync(request, userID);
        return CreatedAtAction(nameof(GetBoardsByWorkspace), new { workspaceID = createdBoard.WorkspaceID }, createdBoard);
    }
    
    [HttpGet("{ID}")]
    public async Task<IActionResult> GetBoardByID(int ID)
    {
        var board = await _boardService.GetBoardByIDAsync(ID);
        
        if (board == null) return NotFound(new { message = "Tableau introuvable." });
        
        return Ok(board);
    }

    [HttpPut("{ID}")]
    public async Task<IActionResult> UpdateBoard(int ID, [FromBody] UpdateBoardRequest request)
    {
        var updatedBoard = await _boardService.UpdateBoardAsync(ID, request);
        if (updatedBoard == null) return NotFound();

        return Ok(updatedBoard);
    }

    [HttpDelete("{ID}")]
    public async Task<IActionResult> DeleteBoard(int ID)
    {
        await _boardService.DeleteBoardAsync(ID);
        return NoContent();
    }
}