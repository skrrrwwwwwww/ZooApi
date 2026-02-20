namespace ZooApi.Infrastructure.Persistence;

public class ZooDbContext(DbContextOptions<ZooDbContext> options) 
    : DbContext(options), IZooDbContext
{
    public DbSet<Animal> Animals => Set<Animal>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ZooDbContext).Assembly);
        modelBuilder.AddTransactionalOutboxEntities();
    }
}