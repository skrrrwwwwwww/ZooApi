using MassTransit;
using Microsoft.EntityFrameworkCore;
using ZooApi.Domain.Entities;

namespace ZooApi.Infrastructure;
public class ZooDbContext : DbContext
{
    public DbSet<Animal> Animals { get; set; } = null!;
    public ZooDbContext(DbContextOptions<ZooDbContext> options) : base(options) { }

    /*protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AnimalConfiguration).Assembly);

        modelBuilder.AddInboxStateEntity();
        modelBuilder.AddOutboxMessageEntity();
        modelBuilder.AddOutboxStateEntity();
    }*/
    
}