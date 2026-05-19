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

    public async Task<IEnumerable<List>> GetListsByBoardIdAsync(int boardId)
    {
        // On récupère toutes les listes liées à cet ID de tableau
        return await _context.Lists
            .Where(l => l.BoardID == boardId)
            .ToListAsync();
    }

    public async Task AddAsync(List list)
    {
        await _context.Lists.AddAsync(list);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}