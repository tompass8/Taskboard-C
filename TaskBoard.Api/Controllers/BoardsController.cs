using System.Security.Claims;
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
    private readonly IBoardService _boardService;

    public BoardsController(IBoardService boardService)
    {
        _boardService = boardService;
    }

    [HttpGet("workspace/{workspaceID}")]
    public async Task<IActionResult> GetBoardsByWorkspace(int workspaceID)
    {
        // Correction de la casse : workspaceID
        var boards = await _boardService.GetBoardsByWorkspaceIDAsync(workspaceID);
        return Ok(boards);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBoard([FromBody] CreateBoardRequest request)
    {
        // 1. Extraction du Guid depuis le Token
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdString, out Guid userId))
            return Unauthorized("Token invalide.");

        // 2. Passage du userId au service
        var createdBoard = await _boardService.CreateBoardAsync(request, userId);
        
        return CreatedAtAction(nameof(GetBoardsByWorkspace), new { workspaceID = createdBoard.WorkspaceID }, createdBoard);
    }

    [HttpPut("{ID:int}")]
    public async Task<IActionResult> UpdateBoard(int ID, [FromBody] UpdateBoardRequest request)
    {
        var updatedBoard = await _boardService.UpdateBoardAsync(ID, request);
        if (updatedBoard == null) return NotFound();

        return Ok(updatedBoard);
    }

    [HttpDelete("{ID:int}")]
    public async Task<IActionResult> DeleteBoard(int ID)
    {
        await _boardService.DeleteBoardAsync(ID);
        return NoContent();
    }
}
