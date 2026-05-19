using TaskBoard.Domain.Entities;

namespace TaskBoard.Application.Interfaces;

public interface IBoardRepository
{
    // On récupère les tableaux liés à un Espace de travail précis (ID en 'int' !)
    Task<IEnumerable<Board>> GetBoardsByWorkspaceIdAsync(int workspaceId);
    
    // On prépare l'ajout d'un nouveau tableau
    Task AddAsync(Board board);
    
    // On sauvegarde les changements
    Task SaveChangesAsync();
}