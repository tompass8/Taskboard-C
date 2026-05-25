namespace TaskBoard.Application.Interfaces;

public interface IBoardNotificationService
{
    Task NotifyCardCreated(int boardID, object card);
    Task NotifyCardUpdated(int boardID, object card);
    Task NotifyCardMoved(int boardID, object card);
    Task NotifyCardDeleted(int boardID, object card);
    
    Task NotifyListCreated(int boardID, object list);
    Task NotifyListUpdated(int boardID, object list);
    Task NotifyListMoved(int boardID, object list);
    Task NotifyListDeleted(int boardID, object list);
}