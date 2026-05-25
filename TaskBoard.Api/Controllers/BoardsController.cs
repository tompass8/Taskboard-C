using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskBoard.Application.DTOs.Boards;
using TaskBoard.Application.Interfaces;

namespace TaskBoard.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BoardsController : ControllerBase
{
    // On injecte le service applicatif à la place du DbContext
    private readonly IBoardService _boardService;

    public BoardsController(IBoardService boardService)
    {
        _boardService = boardService;
    }

    // GET : api/boards/workspace/5
    // On utilise une route spécifique pour récupérer les tableaux d'un espace précis
    [HttpGet("workspace/{workspaceID}")]
    public async Task<IActionResult> GetBoardsByWorkspace(int workspaceID)
    {
        var boards = await _boardService.GetBoardsByWorkspaceIDAsync(workspaceId);
        return Ok(boards);
    }

    // POST : api/boards
    [HttpPost]
    public async Task<IActionResult> CreateBoard([FromBody] CreateBoardRequest request)
    {
        var createdBoard = await _boardService.CreateBoardAsync(request);
        
        // Renvoie un code 201 Created avec le DTO de réponse
        return CreatedAtAction(nameof(GetBoardsByWorkspace), new { workspaceID = createdBoard.WorkspaceID }, createdBoard);
    }

    [HttpPut("{ID}")]
    public async Task<IActionResult> UpdateBoard(int id, [FromBody] UpdateBoardRequest request)
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