using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskBoard.Application.DTOs.Lists;
using TaskBoard.Application.Interfaces;

namespace TaskBoard.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ListsController : ControllerBase
{
    private readonly IListService _listService;
    private readonly IBoardNotificationService _notificationService;

    public ListsController(IListService listService, IBoardNotificationService notificationService)
    {
        _listService = listService;
        _notificationService = notificationService;
    }
    
    // GET : api/lists/board/5
    // Récupère toutes les listes pour un Board spécifique
    [HttpGet("board/{boardID}")]
    public async Task<IActionResult> GetListsByBoard(int boardID)
    {
        var lists = await _listService.GetListsByBoardIdAsync(boardID);
        return Ok(lists);
    }

    // POST : api/lists
    // Crée une nouvelle liste
    [HttpPost]
    public async Task<IActionResult> CreateList([FromBody] CreateListRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        //  BLOC DE TEST TEMPORAIRE
        // En attendant le système d'authentification final
        Guid userID = Guid.Parse("11111111-1111-1111-1111-111111111111");

        var newList = await _listService.CreateListAsync(request, userID);
        
        // On renvoie un code HTTP 201 (Created)
        return CreatedAtAction(nameof(GetListsByBoard), new { boardID = newList.BoardID }, newList);
    }
    
    [HttpPatch("{ID:int}/position")]
    public async Task<IActionResult> UpdateListPosition(int ID, [FromBody] UpdatePositionRequest request)
    {
        // request.Position -> Nouvelle position de la colonne
        // request.BoardID  -> Nécessaire pour cibler le groupe SignalR
        await _listService.UpdateListPositionAsync(ID, request.Position);
        
        await _notificationService.NotifyListMoved(request.BoardID, new
        {
            listID = ID,
            request.Position
        });

        return NoContent();
    }
}