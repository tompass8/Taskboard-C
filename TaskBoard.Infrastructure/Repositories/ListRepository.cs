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

    public async Task<List?> GetByIDAsync(int ID)
    {
        return await _context.Lists.FindAsync(ID);
    }
    
    public async Task<IEnumerable<List>> GetListsByBoardIDAsync(int boardID)
    {
        // On récupère toutes les listes liées à cet ID de tableau
        return await _context.Lists
            .Where(l => l.BoardID == boardID)
            .ToListAsync();
    }
    
    public void Add(List list)
    {
        _context.Lists.Add(list);
    }

    public void Update(List list)
    {
        _context.Lists.Update(list);
    }
    
    public void Delete(List list)
    {
        _context.Lists.Remove(list);
    }
    
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}