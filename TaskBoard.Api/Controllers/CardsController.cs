using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetCard(int id)
    {
        var card = await _cardService.GetCardAsync(id);
        return Ok(card);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCard([FromBody] CreateCardRequest request)
    {
        var card = await _cardService.CreateCardAsync(request);
        await _notificationService.NotifyCardCreated(request.BoardID, card);
        return StatusCode(201, card);
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> UpdateCard(int id, [FromBody] UpdateCardRequest request)
    {
        var card = await _cardService.UpdateCardAsync(id, request);
        await _notificationService.NotifyCardUpdated(request.BoardID, card);
        return Ok(card);
    }
    
    [HttpPatch("{id:int}/position")]
    public async Task<IActionResult> UpdateCardPosition(int id, [FromBody] UpdatePositionRequest request)
    {
        await _cardService.UpdateCardPositionAsync(id, request.Position, request.ListID);
        await _notificationService.NotifyCardMoved(request.BoardID, new
        {
            cardID = id,
            request.Position,
            request.ListID
        });
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteCard(int id, [FromQuery] int boardID)
    {
        await _cardService.DeleteCardAsync(id);
        await _notificationService.NotifyCardDeleted(boardID, new { cardID = id });
        return NoContent();
    }
}