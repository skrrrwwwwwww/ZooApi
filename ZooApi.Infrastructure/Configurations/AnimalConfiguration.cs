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
        
         // Настройка связи с владельцем
        builder.HasOne<Owner>() // У животного есть один владелец
            .WithMany(o => o.Animals) // У владельца много животных
            .HasForeignKey(a => a.OwnerId) // Внешний ключ в таблице Animal
            .IsRequired() // Связь обязательна
            .OnDelete(DeleteBehavior.Cascade); // Удаляем владельца — удаляются и животные
    }
}