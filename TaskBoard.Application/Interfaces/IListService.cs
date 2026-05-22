namespace TaskBoard.Application.Interfaces;

public interface IListService
{
    Task UpdateListPositionAsync(int listId, int newPosition);
}