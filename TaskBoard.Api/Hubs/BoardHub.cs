using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace TaskBoard.Api.Hubs;

[Authorize]
public class BoardHub : Hub
{
    private static string GetGroupName(int boardID)
        => $"board_{boardID}";
    

    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
    }

    public async Task JoinBoard(int boardID)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, GetGroupName(boardID));

        var username = Context.User?.Identity?.Name ?? "Inconnu";
        await Clients.OthersInGroup(GetGroupName(boardID))
            .SendAsync("UserJoined", new
            {
                username,
                boardID,
                connectedAt = DateTime.Now
            });
    }

    public async Task LeaveBoard(int boardID)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, GetGroupName(boardID));

        var username = Context.User?.Identity?.Name ?? "Inconnu";
        await Clients.OthersInGroup(GetGroupName(boardID))
            .SendAsync("UserLeft", new
            {
                username,
                boardID,
            });
    }

    public async Task StartEditingCard(int boardID, int cardID)
    {
        var username = Context.User?.Identity?.Name ?? "Inconnu";
        await Clients.OthersInGroup(GetGroupName(boardID))
            .SendAsync("UserStartEditingCard", new
            {
                username,
                cardID,
                boardID,
            });
    }

    public async Task StopEditingCard(int boardID, int cardID)
    {
        var username = Context.User?.Identity?.Name ?? "Inconnu";
        await Clients.OthersInGroup(GetGroupName(boardID))
            .SendAsync("UserStopEditingCard", new
            {
                username,
                cardID,
                boardID,
            });
    }
}