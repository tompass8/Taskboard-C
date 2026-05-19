using System.ComponentModel.DataAnnotations;

namespace TaskBoard.Application.DTOs.Boards;

public class CreateBoardRequest
{
    [Required(ErrorMessage = "Le nom du tableau est obligatoire.")]
    [StringLength(100, ErrorMessage = "Le nom ne peut pas dépasser 100 caractères.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Le tableau doit être rattaché à un espace de travail (WorkspaceId).")]
    public Guid WorkspaceId { get; set; }
}