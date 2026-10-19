namespace ZooApi.Infrastructure;

public class ZooDbContext(DbContextOptions<ZooDbContext> options)
    : DbContext(options), IZooDbContext
{
    public DbSet<Animal> Animals => Set<Animal>();
    public DbSet<Owner> Owners => Set<Owner>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AnimalConfiguration).Assembly);
        modelBuilder.AddTransactionalOutboxEntities();
    }
}