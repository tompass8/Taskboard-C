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

    public async Task<IEnumerable<Workspace>> GetWorkspacesByUserIdAsync(Guid userId)
    {
        // Récupère les workspaces où l'utilisateur est soit propriétaire, soit membre
        return await _context.Workspaces
            .Include(w => w.Owner)
            .Where(w => w.OwnerId == userId || w.Members.Any(m => m.Id == userId))
            .ToListAsync();
    }

    public async Task<Workspace?> GetByIdAsync(Guid id)
    {
        return await _context.Workspaces
            .Include(w => w.Owner)
            .FirstOrDefaultAsync(w => w.Id == id);
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