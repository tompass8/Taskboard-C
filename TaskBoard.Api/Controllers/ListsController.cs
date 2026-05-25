using System.Security.Claims;
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
    
    [HttpGet("board/{boardID}")]
    public async Task<IActionResult> GetListsByBoard(int boardID)
    {
        // Correction de la casse : GetListsByBoardIDAsync
        var lists = await _listService.GetListsByBoardIDAsync(boardID);
        return Ok(lists);
    }

    [HttpPost]
    public async Task<IActionResult> CreateList([FromBody] CreateListRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Extraction du vrai Guid depuis le Token
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdString, out Guid userID))
            return Unauthorized("Token invalide.");

        var newList = await _listService.CreateListAsync(request, userID);
        
        return CreatedAtAction(nameof(GetListsByBoard), new { boardID = newList.BoardID }, newList);
    }
    
    [HttpPatch("{ID:int}/position")]
    public async Task<IActionResult> UpdateListPosition(int ID, [FromBody] UpdatePositionRequest request)
    {
        await _listService.UpdateListPositionAsync(ID, request.Position);
        
        await _notificationService.NotifyListMoved(request.BoardID, new
        {
            listID = ID,
            request.Position
        });

        return NoContent();
    }
}