using Microsoft.EntityFrameworkCore;
using ZooApi.Domain.Entities;

namespace ZooApi.Infrastructure.Data;
public class ZooDbContext : DbContext
{
    public DbSet<Animal> Animals { get; set; } = null!;

    public ZooDbContext(DbContextOptions<ZooDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Animal>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Species).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Energy).HasDefaultValue(100);
        });
    }
}