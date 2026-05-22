using Microsoft.EntityFrameworkCore;
using TaskBoard.Application.Interfaces;
using TaskBoard.Domain.Entities;
using TaskBoard.Infrastructure.Data;

namespace TaskBoard.Infrastructure.Repositories;

public class BoardRepository : IBoardRepository
{
    private readonly ApplicationDbContext _context;

    public BoardRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Board>> GetBoardsByWorkspaceIdAsync(int workspaceId)
    {
        return await _context.Boards
            .Where(b => b.WorkspaceID == workspaceId) // C'était l'erreur ici !
            .ToListAsync();
    }

    public async Task AddAsync(Board board)
    {
        await _context.Boards.AddAsync(board);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}