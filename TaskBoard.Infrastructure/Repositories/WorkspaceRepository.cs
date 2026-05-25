using Microsoft.EntityFrameworkCore;
using TaskBoard.Application.Interfaces;
using TaskBoard.Domain.Entities;
using TaskBoard.Infrastructure.Data;

namespace TaskBoard.Infrastructure.Repositories;

public class WorkspaceRepository : IWorkspaceRepository
{
    private readonly ApplicationDbContext _context;

    public WorkspaceRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Workspace>> GetUserWorkspacesAsync(Guid userId)
    {
        return await _context.Workspaces
            .Where(w => w.WorkspaceMemberships.Any(wm => wm.UserID == userId))
            .ToListAsync();
    }

    public async Task<Workspace?> GetByIdAsync(int workspaceId)
    {
        return await _context.Workspaces
            .Include(w => w.WorkspaceMemberships)
            .ThenInclude(wm => wm.User)
            .FirstOrDefaultAsync(w => w.ID == workspaceId);
    }

    public async Task<WorkspaceMembership?> GetWorkspaceMembershipAsync(int workspaceId, Guid userId)
    {
        return await _context.WorkspaceMemberships
            .Include(wm => wm.User)
            .FirstOrDefaultAsync(wm => wm.WorkspaceID == workspaceId && wm.UserID == userId);
    }

    public async Task<IEnumerable<WorkspaceMembership>> GetWorkspaceMembersAsync(int workspaceId)
    {
        return await _context.WorkspaceMemberships
            .Include(wm => wm.User)
            .Where(wm => wm.WorkspaceID == workspaceId)
            .ToListAsync();
    }

    public async Task AddAsync(Workspace workspace)
    {
        await _context.Workspaces.AddAsync(workspace);
    }

    public async Task AddWorkspaceMembershipAsync(WorkspaceMembership membership)
    {
        await _context.WorkspaceMemberships.AddAsync(membership);
    }

    public async Task RemoveWorkspaceMembershipAsync(WorkspaceMembership membership)
    {
        _context.WorkspaceMemberships.Remove(membership);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}