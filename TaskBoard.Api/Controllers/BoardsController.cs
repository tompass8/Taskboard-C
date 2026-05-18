using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskBoard.Domain.Entities;
using TaskBoard.Infrastructure.Data;

namespace TaskBoard.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BoardsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public BoardsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/boards
    [HttpGet]
    public async Task<IActionResult> GetBoards()
    {
        var boards = await _context.Boards.ToListAsync();
        return Ok(boards);
    }

    // POST: api/boards
    [HttpPost]
    public async Task<IActionResult> CreateBoard([FromBody] string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            return BadRequest("Titre du tableau requis.");

        var board = new Board { Title = title };
        _context.Boards.Add(board);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetBoards), new { id = board.ID }, board);
    }
}