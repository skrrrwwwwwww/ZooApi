using Microsoft.EntityFrameworkCore;
using ZooApi.Domain.Entities;

namespace ZooApi.Infrastructure.Data;
public class ZooDbContext : DbContext
{
    public DbSet<Animal> Animals { get; set; } = null!;

    public ZooDbContext(DbContextOptions<ZooDbContext> options) : base(options) { }

    
}