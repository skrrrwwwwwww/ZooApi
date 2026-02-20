namespace ZooApi.Infrastructure.Extensions;

public static class RedisExtensions
{
    public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Redis") ?? "redis:6379";
    
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = $"{connectionString},abortConnect=false,connectTimeout=10000,syncTimeout=10000";
            options.InstanceName = "ZooApi";
        });
        return services;
    }
}