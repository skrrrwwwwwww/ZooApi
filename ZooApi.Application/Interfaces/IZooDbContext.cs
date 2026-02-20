namespace ZooApi.Application.Interfaces;

public interface IZooDbContext
{
    DbSet<Animal> Animals { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);    
}