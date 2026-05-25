namespace TaskBoard.Application.DTOs;

public record CreateCardRequest(string Title, int ListID, int BoardID, string? Description, DateTime? Deadline);
public record UpdateCardRequest(string? Title, string? Description, DateTime? Deadline, int BoardID);
public record UpdatePositionRequest(int Position, int BoardID, int? ListID);