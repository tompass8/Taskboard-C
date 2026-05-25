namespace TaskBoard.Application.DTOs.Boards;

public class UpdateBoardRequest
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
}