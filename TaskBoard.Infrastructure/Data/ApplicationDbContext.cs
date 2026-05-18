using Microsoft.EntityFrameworkCore;
using TaskBoard.Domain.Entities;

namespace TaskBoard.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Workspace> Workspaces { get; set; }
    public DbSet<WorkspaceMembership> WorkspaceMemberships { get; set; }
    public DbSet<Board> Boards { get; set; }
    public DbSet<BoardMembership> BoardMemberships { get; set; }
    public DbSet<List> Lists { get; set; }
    public DbSet<Card> Cards { get; set; }
    public DbSet<Label> Labels { get; set; }
    public DbSet<Activity> Activities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ── User ──────────────────────────────────────────────────
        modelBuilder.Entity<User>(e =>
        {
            e.HasKey(u => u.ID);
            e.HasIndex(u => u.Email).IsUnique();
            e.HasIndex(u => u.Username).IsUnique();
        });

        // ── WorkspaceMembership ───────────────────────────────────
        modelBuilder.Entity<WorkspaceMembership>(e =>
        {
            e.HasKey(wm => wm.ID);
            e.HasIndex(wm => new { wm.WorkspaceID, wm.UserID }).IsUnique();

            e.HasOne(wm => wm.Workspace)
                .WithMany(w => w.WorkspaceMemberships)
                .HasForeignKey(wm => wm.WorkspaceID)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(wm => wm.User)
                .WithMany(u => u.WorkspaceMemberships)
                .HasForeignKey(wm => wm.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            e.Property(wm => wm.Role)
                .HasConversion<string>();
        });

        // ── Board ─────────────────────────────────────────────────
        modelBuilder.Entity<Board>(e =>
        {
            e.HasKey(b => b.ID);

            e.HasOne(b => b.Workspace)
                .WithMany(w => w.Boards)
                .HasForeignKey(b => b.WorkspaceID)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(b => b.User)
                .WithMany(u => u.OwnedBoards)
                .HasForeignKey(b => b.OwnerID)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // ── BoardMembership ───────────────────────────────────────
        modelBuilder.Entity<BoardMembership>(e =>
        {
            e.HasKey(bm => bm.ID);
            e.HasIndex(bm => new { bm.BoardID, bm.UserID }).IsUnique();

            e.HasOne(bm => bm.Board)
                .WithMany(b => b.Members)
                .HasForeignKey(bm => bm.BoardID)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(bm => bm.User)
                .WithMany(u => u.BoardMemberships)
                .HasForeignKey(bm => bm.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            e.Property(bm => bm.Role)
                .HasConversion<string>();
        });

        // ── List ──────────────────────────────────────────────────
        modelBuilder.Entity<List>(e =>
        {
            e.HasKey(l => l.ID);

            e.HasOne(l => l.Board)
                .WithMany(b => b.Lists)
                .HasForeignKey(l => l.BoardID)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ── Card ──────────────────────────────────────────────────
        modelBuilder.Entity<Card>(e =>
        {
            e.HasKey(c => c.ID);

            e.HasOne(c => c.List)
                .WithMany(l => l.Cards)
                .HasForeignKey(c => c.ListID)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(c => c.AssignedUser)
                .WithMany(u => u.AssignedCards)
                .HasForeignKey(c => c.AssignedUserID)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // ── Label ─────────────────────────────────────────────────
        modelBuilder.Entity<Label>(e =>
        {
            e.HasKey(l => l.ID);

            e.HasOne(l => l.Card)
                .WithMany(c => c.Labels)
                .HasForeignKey(l => l.CardID)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ── Activity ──────────────────────────────────────────────
        modelBuilder.Entity<Activity>(e =>
        {
            e.HasKey(a => a.ID);

            e.HasOne(a => a.Card)
                .WithMany(c => c.Activities)
                .HasForeignKey(a => a.CardID)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(a => a.User)
                .WithMany(u => u.Activities)
                .HasForeignKey(a => a.UserID)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}