using TaskBoard.Application.DTOs;
using TaskBoard.Domain.Entities;

namespace TaskBoard.Application.Interfaces;

public interface ICardService
{
    Task<Card?> GetCardAsync(int ID);
    Task<Card> CreateCardAsync(CreateCardRequest request,  Guid userID);
    Task<Card?> UpdateCardAsync(int ID, UpdateCardRequest request);
    Task UpdateCardPositionAsync(int cardID, int newPosition, int? newListID);
    Task DeleteCardAsync(int ID);
}