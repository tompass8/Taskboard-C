using TaskBoard.Application.DTOs;
using TaskBoard.Application.Interfaces;
using TaskBoard.Domain.Entities;

namespace TaskBoard.Application.Services;

public class CardService : ICardService
{
    private readonly ICardRepository _cardRepository;

    public CardService(ICardRepository cardRepository)
    {
        _cardRepository = cardRepository;
    }

    public async Task<Card?> GetCardAsync(int ID)
    {
        return await _cardRepository.GetByIDAsync(ID);
    }

    public async Task<Card> CreateCardAsync(CreateCardRequest request)
    {
        // Récupérer les cartes existantes pour placer la nouvelle en fin de liste
        var existingCards = await _cardRepository.GetByListIDAsync(request.ListID);
        int nextPosition = existingCards.Any() ? existingCards.Max(c => c.Position) + 1 : 0;

        var card = new Card
        {
            Title = request.Title,
            Description = request.Description,
            Deadline = request.Deadline,
            ListID = request.ListID,
            Position = nextPosition
        };

        _cardRepository.Add(card);
        await _cardRepository.SaveChangesAsync();
        return card;
    }

    public async Task<Card?> UpdateCardAsync(int ID, UpdateCardRequest request)
    {
        var card = await _cardRepository.GetByIDAsync(ID);
        if (card == null) return null;

        if (request.Title != null) card.Title = request.Title;
        if (request.Description != null) card.Description = request.Description;
        if (request.Deadline != null) card.Deadline = request.Deadline;

        _cardRepository.Update(card);
        await _cardRepository.SaveChangesAsync();
        return card;
    }

    public async Task UpdateCardPositionAsync(int cardID, int newPosition, int? newListID)
    {
        var card = await _cardRepository.GetByIDAsync(cardID);
        if (card == null) return;

        int oldPosition = card.Position;
        int oldListID = card.ListID;
        int targetListID = newListID ?? card.ListID;

        // Cas 1 : Déplacement dans la même colonne (List)
        if (oldListID == targetListID)
        {
            var allCards = await _cardRepository.GetByListIDAsync(oldListID);
            
            if (oldPosition < newPosition)
            {
                foreach (var c in allCards.Where(c => c.Position > oldPosition && c.Position <= newPosition))
                {
                    c.Position--;
                    _cardRepository.Update(c);
                }
            }
            else if (oldPosition > newPosition)
            {
                foreach (var c in allCards.Where(c => c.Position >= newPosition && c.Position < oldPosition))
                {
                    c.Position++;
                    _cardRepository.Update(c);
                }
            }
        }
        // Cas 2 : Déplacement vers une autre colonne (Changement de statut)
        else
        {
            // 1. Décaler vers le bas les cartes de la colonne de destination
            var targetCards = await _cardRepository.GetByListIDAsync(targetListID);
            foreach (var c in targetCards.Where(c => c.Position >= newPosition))
            {
                c.Position++;
                _cardRepository.Update(c);
            }

            // 2. Combler le vide dans la colonne d'origine
            var sourceCards = await _cardRepository.GetByListIDAsync(oldListID);
            foreach (var c in sourceCards.Where(c => c.Position > oldPosition))
            {
                c.Position--;
                _cardRepository.Update(c);
            }

            card.ListID = targetListID;
        }

        card.Position = newPosition;
        _cardRepository.Update(card);
        await _cardRepository.SaveChangesAsync();
    }

    public async Task DeleteCardAsync(int ID)
    {
        var card = await _cardRepository.GetByIDAsync(ID);
        if (card == null) return;

        // Réordonner les cartes restantes sous la carte supprimée
        var siblings = await _cardRepository.GetByListIDAsync(card.ListID);
        foreach (var c in siblings.Where(c => c.Position > card.Position))
        {
            c.Position--;
            _cardRepository.Update(c);
        }

        _cardRepository.Delete(card);
        await _cardRepository.SaveChangesAsync();
    }
}