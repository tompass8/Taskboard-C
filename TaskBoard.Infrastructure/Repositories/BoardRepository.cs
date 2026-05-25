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
    
    public async Task<Board?> GetByIDAsync(int ID)
    {
        return await _context.Boards.FindAsync(ID);
    }

    public async Task<IEnumerable<Board>> GetBoardsByWorkspaceIDAsync(int workspaceID)
    {
        return await _context.Boards
            .Where(b => b.WorkspaceID == workspaceID)
            .ToListAsync();
    }
    
    public void Add(Board board)
    {
        _context.Boards.Add(board);
    }

    public void Update(Board board)
    {
        _context.Boards.Update(board);
    }
    
    public void Delete(Board board)
    {
        _context.Boards.Remove(board);
    }
    
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}