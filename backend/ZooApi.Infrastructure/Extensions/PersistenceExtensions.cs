namespace ZooApi.Infrastructure.Extensions;

public static class PersistenceExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("Строка подключения 'DefaultConnection' не найдена в конфигурации!");
        }

        services.AddDbContext<ZooDbContext>(options 
            => options.UseNpgsql(connectionString)
                .EnableDetailedErrors()
                .EnableSensitiveDataLogging());

        services.AddScoped<IZooDbContext>(provider => provider.GetRequiredService<ZooDbContext>());

        return services;
    }
}