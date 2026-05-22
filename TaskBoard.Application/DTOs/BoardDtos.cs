namespace TaskBoard.Application.DTOs;

public record CreateBoardRequest(string Title, string? Description, int WorkspaceID);
public record UpdateBoardRequest(string? Title, string? Description);
public record BoardResponse(int ID, string Title, string? Description, int WorkspaceID);