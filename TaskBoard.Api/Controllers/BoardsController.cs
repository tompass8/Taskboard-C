using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskBoard.Application.DTOs;
using TaskBoard.Application.Interfaces; // Essentiel pour voir IBoardService

namespace TaskBoard.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BoardsController : ControllerBase // Corrigé : ControllerBase au lieu de ControllerBaseHub
{
    // On injecte le service applicatif à la place du DbContext
    private readonly IBoardService _boardService;

    public BoardsController(IBoardService boardService)
    {
        _boardService = boardService;
    }

    [HttpGet("workspace/{workspaceId}")]
    public async Task<IActionResult> GetBoardsByWorkspace(int workspaceId)
    {
        var boards = await _boardService.GetBoardsByWorkspaceAsync(workspaceId);
        return Ok(boards);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBoard([FromBody] CreateBoardRequest request)
    {
        var createdBoard = await _boardService.CreateBoardAsync(request);
        
        // Renvoie un code 201 Created avec le DTO de réponse
        return CreatedAtAction(nameof(GetBoardsByWorkspace), new { workspaceId = createdBoard.WorkspaceID }, createdBoard);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBoard(int id, [FromBody] UpdateBoardRequest request)
    {
        var updatedBoard = await _boardService.UpdateBoardAsync(id, request);
        if (updatedBoard == null) return NotFound();

        return Ok(updatedBoard);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBoard(int id)
    {
        // Optionnel : Tu peux ajouter une méthode de vérification d'existence ou laisser le service gérer
        await _boardService.DeleteBoardAsync(id);
        return NoContent();
    }
}