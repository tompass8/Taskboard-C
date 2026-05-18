using Microsoft.EntityFrameworkCore;
using TaskBoard.Domain.Entities;

namespace TaskBoard.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    // On déclare officiellement nos tables à la base de données
    public DbSet<Board> Boards { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Workspace> Workspaces { get; set; }

    // N.B : On ajoutera les Listes et Cartes ici lors de la prochaine étape

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 1. Relation Propriétaire (Owner)
        // On connecte explicitement la propriété 'Owner' de Workspace avec la liste 'OwnedWorkspaces' de User
        modelBuilder.Entity<Workspace>()
            .HasOne(w => w.Owner) // <-- Le correctif est ici !
            .WithMany(u => u.OwnedWorkspaces)
            .HasForeignKey(w => w.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);

        // 2. Relation Membres (Members)
        // On connecte explicitement la liste 'Members' de Workspace avec la liste 'Workspaces' de User
        modelBuilder.Entity<Workspace>()
            .HasMany(w => w.Members)
            .WithMany(u => u.Workspaces)
            .UsingEntity(j => j.ToTable("WorkspaceMembers")); 
    }
}