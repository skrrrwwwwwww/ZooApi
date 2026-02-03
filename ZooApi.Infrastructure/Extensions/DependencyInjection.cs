    using MassTransit;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using ZooApi.Application.Interfaces;
    using ZooApi.Application.Services;
    using ZooApi.Infrastructure.Repositories;

    namespace ZooApi.Infrastructure.Extensions;

    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection") 
                                   ?? "Host=localhost;Database=postgres;Username=postgres;Password=password123";
            services.AddDbContext<ZooDbContext>(options => options.UseNpgsql(connectionString));
            
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetConnectionString("Redis") ?? "redis:6379";
                options.InstanceName = "ZooApi";
            });
            
            services.AddMassTransit(x =>
            {
                x.AddConsumers(typeof(AnimalCreatedConsumer).Assembly);
                
                /*x.AddEntityFrameworkOutbox<ZooDbContext>(o =>
                {
                    o.UsePostgres();
                    o.UseBusOutbox();
                    o.QueryDelay = TimeSpan.FromSeconds(1);
                });*/
                
                x.UsingRabbitMq((context, cfg) =>
                {
                    var rabbitSettings = configuration.GetSection("RabbitMq");
                    cfg.Host(rabbitSettings["Host"] ?? "localhost", h => // Для IDE тут будет localhost
                    {
                        h.Username(rabbitSettings["Username"] ?? "guest");
                        h.Password(rabbitSettings["Password"] ?? "guest");
                    });
                    cfg.UseMessageRetry(r  => r.Interval(3, TimeSpan.FromSeconds(5)));
                    cfg.ConfigureEndpoints(context); 
                });
            });
            
            services.AddScoped<IAnimalRepository, AnimalRepository>();
            
            return services;
        }
    }