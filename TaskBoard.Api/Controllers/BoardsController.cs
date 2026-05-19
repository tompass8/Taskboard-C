using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskBoard.Application.DTOs.Boards;
using TaskBoard.Application.Interfaces;

namespace TaskBoard.Api.Controllers;

// [Authorize] // ⚠️ Retire les '//' quand ton système de connexion JWT sera prêt !
[ApiController]
[Route("api/[controller]")]
public class BoardsController : ControllerBase
{
    private readonly IBoardService _boardService;

    public BoardsController(IBoardService boardService)
    {
        _boardService = boardService;
    }

    // GET : api/boards/workspace/5
    // On utilise une route spécifique pour récupérer les tableaux d'un espace précis
    [HttpGet("workspace/{workspaceId}")]
    public async Task<IActionResult> GetBoards(int workspaceId)
    {
        var boards = await _boardService.GetBoardsByWorkspaceIdAsync(workspaceId);
        return Ok(boards);
    }

    // POST : api/boards
    [HttpPost]
    public async Task<IActionResult> CreateBoard([FromBody] CreateBoardRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // 🛑 BLOC DE TEST : À SUPPRIMER QUAND L'AUTH SERA CODÉE
        // On force un Guid puisque Lucas a défini OwnerID comme un Guid dans l'entité Board
        Guid userId = Guid.Parse("11111111-1111-1111-1111-111111111111");

        /* 🟢 VRAI CODE À DÉCOMMENTER PLUS TARD
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdString, out Guid userId))
            return Unauthorized("Token invalide ou utilisateur introuvable.");
        */

        var newBoard = await _boardService.CreateBoardAsync(request, userId);
        
        // Renvoie un code 201 Created avec l'URL pour consulter le nouveau tableau
        return CreatedAtAction(nameof(GetBoards), new { workspaceId = newBoard.WorkspaceID }, newBoard);
    }
}