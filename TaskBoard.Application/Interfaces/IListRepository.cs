using TaskBoard.Domain.Entities;

namespace TaskBoard.Application.Interfaces;

public interface IListRepository
{
    // Récupérer toutes les listes qui appartiennent à un tableau précis
    Task<IEnumerable<List>> GetListsByBoardIdAsync(int boardId);
    
    // Ajouter une nouvelle liste
    Task AddAsync(List list);
    
    // Sauvegarder les modifications en base
    Task SaveChangesAsync();
}