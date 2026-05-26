using System.ComponentModel.DataAnnotations;
using TaskBoard.Application.DTOs;

namespace TaskBoard.Application.DTOs.Lists;

public class ListDto
{
    public int ID { get; set; }
    public string Title { get; set; } = string.Empty;
    public int BoardID { get; set; }
    public List<CardDto> Cards { get; set; } = new();
}

public class CreateListRequest
{
    [Required(ErrorMessage = "Le titre de la liste est obligatoire.")]
    [StringLength(100, ErrorMessage = "Le titre ne peut pas dépasser 100 caractères.")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "La liste doit être rattachée à un tableau (BoardID).")]
    public int BoardID { get; set; }
}