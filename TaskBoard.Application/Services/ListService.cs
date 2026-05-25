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
        // Récupère les listes existantes pour définir la position de la nouvelle colonne
        var existingLists = await _listRepository.GetListsByBoardIDAsync(request.BoardID);
        int nextPosition = existingLists.Count(); // Si 0 listes -> position 0, si 2 listes -> position 2 (index 0, 1, 2)

        var list = new List
        {
            Title = request.Title,
            BoardID = request.BoardID,
            Position = nextPosition // Assure la persistance pour le Drag & Drop
        };

        _listRepository.Add(list);
        await _listRepository.SaveChangesAsync();

        return new ListDto
        {
            ID = list.ID,
            Title = list.Title,
            BoardID = list.BoardID
        };
    }
    
    public async Task UpdateListPositionAsync(int listID, int newPosition)
    {
        // Récupère la liste cible à déplacer
        var list = await _listRepository.GetByIDAsync(listID);
        if (list == null) return;

        int oldPosition = list.Position;
        int boardID = list.BoardID;

        // Récupère toutes les listes du même tableau pour calculer les décalages
        var allLists = await _listRepository.GetListsByBoardIDAsync(boardID);

        // Cas A : Déplacement de la colonne vers la droite (ex: position 1 à 3)
        if (oldPosition < newPosition)
        {
            var listsToShift = allLists.Where(l => l.Position > oldPosition && l.Position <= newPosition);
            foreach (var l in listsToShift)
            {
                l.Position--;
                _listRepository.Update(l); // Prépare le décalage vers la gauche
            }
        }
        // Cas B : Déplacement de la colonne vers la gauche (ex: position 3 à 1)
        else if (oldPosition > newPosition)
        {
            var listsToShift = allLists.Where(l => l.Position >= newPosition && l.Position < oldPosition);
            foreach (var l in listsToShift)
            {
                l.Position++;
                _listRepository.Update(l); // Prépare le décalage vers la droite
            }
        }

        // Assigne la nouvelle position finale à la liste déplacée
        list.Position = newPosition;
        _listRepository.Update(list);

        // Sauvegarde toutes les modifications en une seule transaction SQL
        await _listRepository.SaveChangesAsync();
    }
}