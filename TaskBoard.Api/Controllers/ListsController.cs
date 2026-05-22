using Microsoft.AspNetCore.Mvc;
using TaskBoard.Application.DTOs.Lists;
using TaskBoard.Application.Interfaces;

namespace TaskBoard.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ListsController : ControllerBase
{
    private readonly IListService _listService;

    public ListsController(IListService listService)
    {
        _listService = listService;
    }

    // GET : api/lists/board/5
    // Récupère toutes les listes pour un Board spécifique
    [HttpGet("board/{boardId}")]
    public async Task<IActionResult> GetListsByBoard(int boardId)
    {
        var lists = await _listService.GetListsByBoardIdAsync(boardId);
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
        Guid userId = Guid.Parse("11111111-1111-1111-1111-111111111111");

        var newList = await _listService.CreateListAsync(request, userId);
        
        // On renvoie un code HTTP 201 (Created)
        return CreatedAtAction(nameof(GetListsByBoard), new { boardId = newList.BoardID }, newList);
    }
}