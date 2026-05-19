namespace TaskBoard.Application.DTOs.Boards;

public class BoardDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid WorkspaceId { get; set; }
    public DateTime CreatedAt { get; set; }
}