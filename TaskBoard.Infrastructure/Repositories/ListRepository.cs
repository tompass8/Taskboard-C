using Microsoft.EntityFrameworkCore;
using TaskBoard.Application.Interfaces;
using TaskBoard.Domain.Entities;
using TaskBoard.Infrastructure.Data;

namespace TaskBoard.Infrastructure.Repositories;

public class ListRepository : IListRepository
{
    private readonly ApplicationDbContext _context;

    public ListRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List?> GetByIdAsync(int id)
    {
        return await _context.Lists.FindAsync(id);
    }

    public async Task<IEnumerable<List>> GetByBoardIdAsync(int boardId)
    {
        return await _context.Lists
            .Where(l => l.BoardID == boardId)
            .ToListAsync();
    }

    public void Update(List list)
    {
        _context.Lists.Update(list);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}