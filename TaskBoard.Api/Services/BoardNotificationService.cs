using Microsoft.AspNetCore.SignalR;
using TaskBoard.Api.Hubs;
using TaskBoard.Application.Interfaces;

namespace TaskBoard.Api.Services;

public class BoardNotificationService : IBoardNotificationService
{
    private readonly IHubContext<BoardHub> _hubContext;
    
    /// <summary>
    /// Centralise l'envoi des notifications SignalR pour un tableau spécifique.
    /// </summary>
    private async Task SendAsync(int boardID, string method, object data)
        => await _hubContext.Clients
            .Group($"board_{boardID}")
            .SendAsync(method, data);

    public BoardNotificationService(IHubContext<BoardHub> hubContext)
    {
        _hubContext = hubContext;
    }
    
    // --- Actions sur les Cartes ---
    public async Task NotifyCardCreated(int boardID, object card)
        => await SendAsync(boardID, "CardCreated", card);

    public async Task NotifyCardMoved(int boardID, object card)
        => await SendAsync(boardID, "CardMoved", card);

    public async Task NotifyCardUpdated(int boardID, object card)
        => await SendAsync(boardID, "CardUpdated", card);

    public async Task NotifyCardDeleted(int boardID, object card)
        => await SendAsync(boardID, "CardDeleted", card);
    
    // --- Actions sur les Listes ---
    public async Task NotifyListCreated(int boardID, object list)
        => await SendAsync(boardID, "ListCreated", list);

    public async Task NotifyListUpdated(int boardID, object list)
        => await SendAsync(boardID, "ListUpdated", list);

    public async Task NotifyListMoved(int boardID, object list)
        => await SendAsync(boardID, "ListMoved", list);

    public async Task NotifyListDeleted(int boardID, object list)
        => await SendAsync(boardID, "ListDeleted", list);
}