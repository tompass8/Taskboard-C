namespace TaskBoard.Domain.Entities;

public class Board
{
    public int ID { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Relations Foreign keys
    public int WorkspaceID { get; set; }
    public Guid OwnerID { get; set; }
    
    // Null-forgiving operator (null!) : tells the compiler that EF Core
        // will populate this property at runtime, suppressing warning CS8618.
    public Workspace Workspace { get; set; } = null!;
    public User User { get; set; } = null!;
    
    // ICollection<T> : interface representing a navigable collection of related entities.
        // Used instead of List<T> to stay independent of the concrete implementation,
        // allowing EF Core to substitute its own optimized proxy at runtime.
        // Initialized with new List<T>() to avoid null reference exceptions
        // before EF Core loads the data.
        
        // Un Board PEUT exister sans membres ou sans listes.
        // On utilise l'interface ICollection<T> plutôt que la classe concrète List<T>
        // pour rester indépendant de l'implémentation — EF Core peut substituer
        // sa propre collection optimisée (proxy) au moment du chargement des données.
        // ICollection<T> expose les opérations essentielles : Add(), Remove(), Count, Contains()
        // ce qui est suffisant pour manipuler des relations en EF Core.
        // On initialise avec new List<T>() pour éviter les NullReferenceException
        // avant qu'EF Core charge les données via .Include().
    public ICollection<BoardMembership> Members { get; set; } = new List<BoardMembership>();
    public ICollection<List> Lists { get; set; } = new List<List>();
}