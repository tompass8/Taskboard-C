using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskBoard.Api.Extensions;
using TaskBoard.Application.Interfaces;
using TaskBoard.Application.DTOs;


namespace TaskBoard.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CardsController : ControllerBase
{
    private readonly ICardService _cardService;
    private readonly IBoardNotificationService _notificationService;

    public CardsController(
        ICardService cardService,
        IBoardNotificationService notificationService)
    {
        _cardService = cardService;
        _notificationService = notificationService;
    }

    [HttpGet("{ID:int}")]
    public async Task<IActionResult> GetCard(int ID)
    {
        var card = await _cardService.GetCardAsync(ID);
        return Ok(card);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCard([FromBody] CreateCardRequest request)
    {
        Guid userID = User.GetUserID();
        
        var card = await _cardService.CreateCardAsync(request,  userID);
        await _notificationService.NotifyCardCreated(request.BoardID, card);
        return StatusCode(201, card);
    }

    [HttpPut("{ID}")]
    public async Task<IActionResult> UpdateCard(int ID, [FromBody] UpdateCardRequest request)
    {
        var card = await _cardService.UpdateCardAsync(ID, request);
    
        if (card == null) return NotFound();
        await _notificationService.NotifyCardUpdated(card.ListID, card);
    
        return Ok(card);
    }
    
    [HttpPatch("{ID:int}/position")]
    public async Task<IActionResult> UpdateCardPosition(int ID, [FromBody] UpdatePositionRequest request)
    {
        await _cardService.UpdateCardPositionAsync(ID, request.Position, request.ListID);
        await _notificationService.NotifyCardMoved(request.BoardID, new
        {
            cardID = ID,
            request.Position,
            request.ListID
        });
        return NoContent();
    }

    [HttpDelete("{ID:int}")]
    public async Task<IActionResult> DeleteCard(int ID, [FromQuery] int boardID)
    {
        await _cardService.DeleteCardAsync(ID);
        await _notificationService.NotifyCardDeleted(boardID, new { cardID = ID });
        return NoContent();
    }
}