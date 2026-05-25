using TaskBoard.Domain.Entities;

namespace TaskBoard.Application.Interfaces;

public interface ICardRepository
{
    Task<Card?> GetByIDAsync(int ID);
    Task<IEnumerable<Card>> GetByListIDAsync(int listID);
    void Add(Card card);
    void Update(Card card);
    void Delete(Card card);
    Task SaveChangesAsync();
}