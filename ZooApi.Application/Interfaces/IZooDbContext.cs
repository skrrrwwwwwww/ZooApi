namespace ZooApi.Application.Interfaces;

public interface IZooDbContext
{
    DbSet<Animal> Animals { get; }
    DbSet<Owner> Owners { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}