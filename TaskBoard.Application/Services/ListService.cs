using TaskBoard.Application.DTOs.Lists;
using TaskBoard.Application.Interfaces;
using TaskBoard.Domain.Entities;

namespace TaskBoard.Application.Services;

public class ListService : IListService
{
    private readonly IListRepository _listRepository;

    public ListService(IListRepository listRepository)
    {
        _listRepository = listRepository;
    }

    public async Task<IEnumerable<ListDto>> GetListsByBoardIdAsync(int boardId)
    {
        var lists = await _listRepository.GetListsByBoardIdAsync(boardId);
        
        // On transforme les entités brutes en DTOs
        return lists.Select(l => new ListDto
        {
            ID = l.ID,
            Title = l.Title, // On suppose que Lucas a utilisé 'Title' comme pour les Boards
            BoardID = l.BoardID
        });
    }

    public async Task<ListDto> CreateListAsync(CreateListRequest request, Guid userId)
    {
        // On instancie la vraie entité du Domaine
        var list = new List
        {
            Title = request.Title,
            BoardID = request.BoardID
        };

        // On sauvegarde via le Repository
        await _listRepository.AddAsync(list);
        await _listRepository.SaveChangesAsync();

        // On retourne l'objet propre pour l'API
        return new ListDto
        {
            ID = list.ID,
            Title = list.Title,
            BoardID = list.BoardID
        };
    }
}