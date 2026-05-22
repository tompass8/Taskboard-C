using TaskBoard.Application.DTOs.Lists;

namespace TaskBoard.Application.Interfaces;

public interface IListService
{
    // L'API demandera les listes prêtes à l'emploi (DTO) pour un tableau donné
    Task<IEnumerable<ListDto>> GetListsByBoardIdAsync(int boardId);
    
    // L'API demandera de créer une liste en envoyant le formulaire et l'ID de l'utilisateur
    Task<ListDto> CreateListAsync(CreateListRequest request, Guid userId);
}