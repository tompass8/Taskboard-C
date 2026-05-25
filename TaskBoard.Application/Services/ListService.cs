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

    public async Task<IEnumerable<ListDto>> GetListsByBoardIDAsync(int boardID)
    {
        var lists = await _listRepository.GetListsByBoardIDAsync(boardID);
        
        // On transforme les entités brutes en DTOs
        return lists.Select(l => new ListDto
        {
            ID = l.ID,
            Title = l.Title,
            BoardID = l.BoardID
        });
    }

    public async Task<ListDto> CreateListAsync(CreateListRequest request, Guid userID)
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
    
    public async Task UpdateListPositionAsync(int listID, int newPosition)
    {
        // 1. Récupérer la liste (colonne) cible à déplacer
        var list = await _listRepository.GetByIdAsync(listID);
        if (list == null) return;

        int oldPosition = list.Position;
        int boardId = list.BoardID;

        // 2. Récupérer toutes les listes du même tableau pour calculer les décalages
        var allLists = await _listRepository.GetByBoardIdAsync(boardID);

        // 3. Cas A : Déplacement de la colonne vers la droite (ex: position 1 à 3)
        if (oldPosition < newPosition)
        {
            var listsToShift = allLists.Where(l => l.Position > oldPosition && l.Position <= newPosition);
            foreach (var l in listsToShift)
            {
                l.Position--;
                _listRepository.Update(l); // Prépare le décalage vers la gauche
            }
        }
        // 4. Cas B : Déplacement de la colonne vers la gauche (ex: position 3 à 1)
        else if (oldPosition > newPosition)
        {
            var listsToShift = allLists.Where(l => l.Position >= newPosition && l.Position < oldPosition);
            foreach (var l in listsToShift)
            {
                l.Position++;
                _listRepository.Update(l); // Prépare le décalage vers la droite
            }
        }

        // 5. Assigner la nouvelle position finale à la liste déplacée
        list.Position = newPosition;
        _listRepository.Update(list);

        // 6. Sauvegarder toutes les modifications en une seule transaction SQL
        await _listRepository.SaveChangesAsync();
    }
}