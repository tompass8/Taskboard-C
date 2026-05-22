using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskBoard.Application.DTOs;
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

    [HttpPatch("{id:int}/position")]
    public async Task<IActionResult> UpdateListPosition(int id, [FromBody] UpdatePositionRequest request)
    {
        // request.Position -> Nouvelle position de la colonne
        // request.BoardID  -> Nécessaire pour cibler le groupe SignalR
        await _listService.UpdateListPositionAsync(id, request.Position);
        
        await _notificationService.NotifyListMoved(request.BoardID, new
        {
            listID = id,
            request.Position
        });

        return NoContent();
    }
}