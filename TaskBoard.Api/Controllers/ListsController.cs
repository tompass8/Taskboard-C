using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskBoard.Api.Extensions;
using TaskBoard.Application.DTOs;
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
    
    // Récupère toutes les listes pour un Board spécifique
    [HttpGet("board/{boardID}")]
    public async Task<IActionResult> GetListsByBoard(int boardID)
    {
        var lists = await _listService.GetListsByBoardIDAsync(boardID);
        return Ok(lists);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateList([FromBody] CreateListRequest request)
    {
        Guid userID = User.GetUserID();

        var newList = await _listService.CreateListAsync(request, userID);
        await _notificationService.NotifyListCreated(request.BoardID, newList);

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