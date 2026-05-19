using TaskBoard.Application.DTOs.Workspaces;
using TaskBoard.Application.Interfaces;
using TaskBoard.Domain.Entities;

namespace TaskBoard.Application.Services;

public class WorkspaceService : IWorkspaceService
{
    private readonly IWorkspaceRepository _workspaceRepository;

    // L'injection de dépendance : le service demande un accès au repository
    public WorkspaceService(IWorkspaceRepository workspaceRepository)
    {
        _workspaceRepository = workspaceRepository;
    }

    public async Task<IEnumerable<WorkspaceDto>> GetUserWorkspacesAsync(Guid userId)
    {
        // 1. On va chercher les données brutes
        var workspaces = await _workspaceRepository.GetWorkspacesByUserIdAsync(userId);

        // 2. On les transforme en DTOs légers pour le frontend
        return workspaces.Select(w => new WorkspaceDto
        {
            Id = w.Id,
            Name = w.Name,
            Description = w.Description,
            OwnerId = w.OwnerId,
            CreatedAt = w.CreatedAt
        });
    }

    public async Task<WorkspaceDto> CreateWorkspaceAsync(CreateWorkspaceRequest request, Guid userId)
    {
        // 1. On crée la "vraie" entité à partir de la requête (le DTO d'entrée)
        var workspace = new Workspace
        {
            Id = Guid.NewGuid(), // On génère un nouvel identifiant unique
            Name = request.Name,
            Description = request.Description,
            OwnerId = userId,
            CreatedAt = DateTime.UtcNow // On fixe la date de création à maintenant
        };

        // 2. On demande au Repository de sauvegarder ça en base de données
        await _workspaceRepository.AddAsync(workspace);
        await _workspaceRepository.SaveChangesAsync();

        // 3. On renvoie le résultat propre (le DTO de sortie)
        return new WorkspaceDto
        {
            Id = workspace.Id,
            Name = workspace.Name,
            Description = workspace.Description,
            OwnerId = workspace.OwnerId,
            CreatedAt = workspace.CreatedAt
        };
    }
}