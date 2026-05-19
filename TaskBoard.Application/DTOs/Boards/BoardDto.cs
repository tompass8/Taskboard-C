namespace TaskBoard.Application.DTOs.Boards;

public class BoardDto
{
    public int ID { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int WorkspaceID { get; set; }
    public DateTime CreatedAt { get; set; }
}