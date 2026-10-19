namespace ZooApi.Infrastructure.Configurations;

public class OwnerConfiguration : IEntityTypeConfiguration<Owner>
{
    public void Configure(EntityTypeBuilder<Owner> builder){
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.ContactInfo)
            .IsRequired()
            .HasMaxLength(500);

        // Доступ к приватному полю коллекции для EF
        builder.Metadata
            .FindNavigation(nameof(Owner.Animals))?
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}