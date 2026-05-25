using TaskBoard.Application.DTOs.Workspaces;
using TaskBoard.Application.Interfaces;
using TaskBoard.Domain.Entities;

namespace TaskBoard.Application.Services;

public class WorkspaceService : IWorkspaceService
{
    private readonly IWorkspaceRepository _workspaceRepository;

    public WorkspaceService(IWorkspaceRepository workspaceRepository)
    {
        _workspaceRepository = workspaceRepository;
    }

    public async Task<IEnumerable<WorkspaceDto>> GetUserWorkspacesAsync(Guid userId)
    {
        var workspaces = await _workspaceRepository.GetUserWorkspacesAsync(userId);
        return workspaces.Select(w => new WorkspaceDto
        {
            ID = w.ID,
            Name = w.Name,
            Description = w.Description
        });
    }

    public async Task<WorkspaceDto> CreateWorkspaceAsync(CreateWorkspaceRequest request, Guid userId)
    {
        var workspace = new Workspace
        {
            Name = request.Name,
            Description = request.Description
        };

        workspace.WorkspaceMemberships.Add(new WorkspaceMembership
        {
            UserID = userId,
            Role = Role.Admin,
            Workspace = workspace
        });

        await _workspaceRepository.AddAsync(workspace);
        await _workspaceRepository.SaveChangesAsync();

        return new WorkspaceDto
        {
            ID = workspace.ID,
            Name = workspace.Name,
            Description = workspace.Description
        };
    }
}