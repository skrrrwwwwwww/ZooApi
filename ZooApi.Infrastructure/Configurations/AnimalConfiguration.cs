using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZooApi.Domain.Entities;

namespace ZooApi.Infrastructure.Configurations;
public class AnimalConfiguration : IEntityTypeConfiguration<Animal>
{
    public void Configure(EntityTypeBuilder<Animal> builder)
    {
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(e => e.Species)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(e => e.Energy)
            .HasDefaultValue(100);
    }
}