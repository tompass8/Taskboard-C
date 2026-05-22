namespace TaskBoard.Application.DTOs.Lists;

public class ListDto
{
    public int ID { get; set; }
    public string Title { get; set; } = string.Empty;
    public int BoardID { get; set; }
}