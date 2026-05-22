using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskBoard.Application.Interfaces;
using TaskBoard.Application.DTOs;


namespace TaskBoard.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BoardsController : ControllerBase
{
    private readonly IBoardService _boardService;
    private readonly IBoardNotificationService _notificationService;

    public BoardsController
        (
        IBoardService boardService,
        IBoardNotificationService notificationService
        )
    {
        _boardService = boardService;
        _notificationService = notificationService;
    }

    [HttpGet("{workspaceId:int}")]
    public async Task<IActionResult> GetBoards(int workspaceId)
    {
        var boards = await _boardService.GetBoardsByWorkspaceAsync(workspaceId);
        return Ok(boards);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBoard([FromBody] CreateBoardRequest request)
    {
        var board = await _boardService.CreateBoardAsync(request);
        await _notificationService.NotifyCardCreated(board.ID, board);
        return StatusCode(201, board);
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> UpdateBoard(int id, [FromBody] UpdateBoardRequest request)
    {
        var board = await _boardService.UpdateBoardAsync(id, request);
        return Ok(board);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteBoard(int id)
    {
        await _boardService.DeleteBoardAsync(id);
        return NoContent();
    }
}