using TaskBoard.Application.DTOs.Workspaces;
using TaskBoard.Application.Interfaces;
using TaskBoard.Domain.Entities;
using TaskBoard.Domain.Interfaces;

namespace TaskBoard.Application.Services;

public class WorkspaceService : IWorkspaceService
{
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly IUserRepository _userRepository;

    public WorkspaceService(
        IWorkspaceRepository workspaceRepository,
        IUserRepository userRepository)
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

    public async Task<IEnumerable<WorkspaceMembershipDto>> GetWorkspaceMembersAsync(int workspaceId, Guid requesterId)
    {
        await EnsureWorkspaceExistsAsync(workspaceId);
        await EnsureRequesterIsMemberAsync(workspaceId, requesterId);

        var memberships = await _workspaceRepository.GetWorkspaceMembersAsync(workspaceId);
        return memberships.Select(m => new WorkspaceMembershipDto
        {
            ID = m.ID,
            WorkspaceID = m.WorkspaceID,
            UserID = m.UserID,
            Username = m.User.Username,
            Email = m.User.Email,
            Role = m.Role
        });
    }

    public async Task<WorkspaceMembershipDto> AddWorkspaceMemberAsync(int workspaceId, CreateWorkspaceMembershipRequest request, Guid requesterId)
    {
        var workspace = await EnsureWorkspaceExistsAsync(workspaceId);
        await EnsureRequesterIsAdminAsync(workspaceId, requesterId);

        if (await _workspaceRepository.GetWorkspaceMembershipAsync(workspaceId, request.UserID) != null)
            throw new InvalidOperationException("Utilisateur déjà membre de cet espace de travail.");

        var user = await _userRepository.GetUserByIdAsync(request.UserID)
            ?? throw new KeyNotFoundException("Utilisateur introuvable.");

        var membership = new WorkspaceMembership
        {
            WorkspaceID = workspaceId,
            UserID = user.ID,
            Role = request.Role,
            Workspace = workspace,
            User = user
        };

        await _workspaceRepository.AddWorkspaceMembershipAsync(membership);
        await _workspaceRepository.SaveChangesAsync();

        return new WorkspaceMembershipDto
        {
            ID = membership.ID,
            WorkspaceID = membership.WorkspaceID,
            UserID = membership.UserID,
            Username = user.Username,
            Email = user.Email,
            Role = membership.Role
        };
    }

    public async Task<WorkspaceMembershipDto> UpdateWorkspaceMemberRoleAsync(int workspaceId, Guid memberUserId, UpdateWorkspaceMembershipRoleRequest request, Guid requesterId)
    {
        await EnsureWorkspaceExistsAsync(workspaceId);
        await EnsureRequesterIsAdminAsync(workspaceId, requesterId);

        var membership = await _workspaceRepository.GetWorkspaceMembershipAsync(workspaceId, memberUserId)
            ?? throw new KeyNotFoundException("Membre introuvable dans cet espace de travail.");

        if (membership.Role == Role.Admin && request.Role != Role.Admin)
        {
            var admins = (await _workspaceRepository.GetWorkspaceMembersAsync(workspaceId))
                .Count(m => m.Role == Role.Admin);
            if (admins <= 1)
                throw new InvalidOperationException("Impossible de retirer le dernier administrateur.");
        }

        membership.Role = request.Role;
        await _workspaceRepository.SaveChangesAsync();

        return new WorkspaceMembershipDto
        {
            ID = membership.ID,
            WorkspaceID = membership.WorkspaceID,
            UserID = membership.UserID,
            Username = membership.User.Username,
            Email = membership.User.Email,
            Role = membership.Role
        };
    }

    public async Task RemoveWorkspaceMemberAsync(int workspaceId, Guid memberUserId, Guid requesterId)
    {
        await EnsureWorkspaceExistsAsync(workspaceId);
        await EnsureRequesterIsAdminAsync(workspaceId, requesterId);

        var membership = await _workspaceRepository.GetWorkspaceMembershipAsync(workspaceId, memberUserId)
            ?? throw new KeyNotFoundException("Membre introuvable dans cet espace de travail.");

        if (membership.Role == Role.Admin)
        {
            var admins = (await _workspaceRepository.GetWorkspaceMembersAsync(workspaceId))
                .Count(m => m.Role == Role.Admin);
            if (admins <= 1)
                throw new InvalidOperationException("Impossible de supprimer le dernier administrateur.");
        }

        await _workspaceRepository.RemoveWorkspaceMembershipAsync(membership);
        await _workspaceRepository.SaveChangesAsync();
    }

    private async Task<Workspace> EnsureWorkspaceExistsAsync(int workspaceId)
    {
        var workspace = await _workspaceRepository.GetByIdAsync(workspaceId);
        if (workspace == null)
            throw new KeyNotFoundException("Workspace introuvable.");

        return workspace;
    }

    private async Task<WorkspaceMembership> EnsureRequesterIsMemberAsync(int workspaceId, Guid requesterId)
    {
        var membership = await _workspaceRepository.GetWorkspaceMembershipAsync(workspaceId, requesterId);
        if (membership == null)
            throw new UnauthorizedAccessException("Vous n'êtes pas membre de cet espace de travail.");

        return membership;
    }

    private async Task<WorkspaceMembership> EnsureRequesterIsAdminAsync(int workspaceId, Guid requesterId)
    {
        var membership = await EnsureRequesterIsMemberAsync(workspaceId, requesterId);
        if (membership.Role != Role.Admin)
            throw new UnauthorizedAccessException("Vous n'avez pas le droit de gérer les membres de cet espace de travail.");

        return membership;
    }
}