using System.ComponentModel.DataAnnotations;

namespace TaskBoard.Application.DTOs.Boards;

public class CreateBoardRequest
{
    [Required(ErrorMessage = "Le titre du tableau est obligatoire.")]
    [StringLength(100, ErrorMessage = "Le titre ne peut pas dépasser 100 caractères.")]
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    [Required(ErrorMessage = "Le tableau doit être rattaché à un espace de travail.")]
    public int WorkspaceID { get; set; }
}