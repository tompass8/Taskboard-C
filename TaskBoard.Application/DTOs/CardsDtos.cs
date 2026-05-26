namespace TaskBoard.Application.DTOs;

public class CardDto
{
    public int ID { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Position { get; set; }
    public int ListID { get; set; }
    public DateTime? Deadline { get; set; }
}
public record CreateCardRequest(string Title, int ListID, int BoardID, string? Description, DateTime? Deadline);
public record UpdateCardRequest(string? Title, string? Description, DateTime? Deadline, int BoardID);
public record UpdatePositionRequest(int Position, int BoardID, int? ListID);