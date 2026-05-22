using TaskBoard.Application.DTOs;

namespace TaskBoard.Application.Interfaces;

public interface ICardService
{
    Task<object> GetCardAsync(int id);
    Task<object> CreateCardAsync(CreateCardRequest request);
    Task<object> UpdateCardAsync(int id, UpdateCardRequest request);
    Task UpdateCardPositionAsync(int id, int position, int? listID);
    Task DeleteCardAsync(int id);
}