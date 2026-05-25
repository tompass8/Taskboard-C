using Microsoft.EntityFrameworkCore;
using TaskBoard.Application.Interfaces;
using TaskBoard.Domain.Entities;
using TaskBoard.Infrastructure.Data;

namespace TaskBoard.Infrastructure.Repositories;

public class CardRepository : ICardRepository
{
    private readonly ApplicationDbContext _context;

    public CardRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Card?> GetByIDAsync(int id)
    {
        return await _context.Cards.FindAsync(id);
    }

    public async Task<IEnumerable<Card>> GetByListIDAsync(int listID)
    {
        return await _context.Cards
            .Where(c => c.ListID == listID)
            .OrderBy(c => c.Position)
            .ToListAsync();
    }

    public void Add(Card card)
    {
        _context.Cards.Add(card);
    }

    public void Update(Card card)
    {
        _context.Cards.Update(card);
    }

    public void Delete(Card card)
    {
        _context.Cards.Remove(card);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}