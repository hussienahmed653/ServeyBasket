namespace ServeyBasket.Persistense;

public class ServeyBasketDbContext(DbContextOptions<ServeyBasketDbContext> options) : DbContext(options)
{
    public DbSet<Poll> Polls { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}
