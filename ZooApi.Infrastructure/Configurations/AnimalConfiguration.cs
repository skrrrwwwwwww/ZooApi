using Microsoft.EntityFrameworkCore;
using ZooApi.Domain.Entities;

namespace ZooApi.Infrastructure.Configurations;

public class AnimalConfiguration
{
    protected void OnModelCreating(ModelBuilder modelBuilder)
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