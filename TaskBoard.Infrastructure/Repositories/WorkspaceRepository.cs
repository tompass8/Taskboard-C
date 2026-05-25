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
        // Sécurité : On retourne uniquement les workspaces liés à ce Guid
        return await _context.Workspaces
            .Where(w => w.WorkspaceMemberships.Any(m => m.UserID == userId))
            .ToListAsync();
    }

    public async Task AddAsync(Workspace workspace)
    {
        await _context.Workspaces.AddAsync(workspace);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
