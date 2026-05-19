using TaskBoard.Application.DTOs.Boards;

namespace TaskBoard.Application.Interfaces;

public interface IBoardService
{
    // L'API demandera la liste des tableaux propres (BoardDto) d'un espace de travail
    Task<IEnumerable<BoardDto>> GetBoardsByWorkspaceIdAsync(int workspaceId);
    
    // L'API demandera de créer un tableau en fournissant le formulaire (Request) et l'ID du créateur
    Task<BoardDto> CreateBoardAsync(CreateBoardRequest request, int userId);
}