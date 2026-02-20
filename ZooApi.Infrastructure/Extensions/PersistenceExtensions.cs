namespace ZooApi.Infrastructure.Extensions;

public static class PersistenceExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection") 
                               ?? "Host=localhost;Database=postgres;Username=postgres;Password=password123";
    
        services.AddDbContext<ZooDbContext>(options => options.UseNpgsql(connectionString));
        services.AddScoped<IZooDbContext>(provider => provider.GetRequiredService<ZooDbContext>());
        
        return services;
    }
}