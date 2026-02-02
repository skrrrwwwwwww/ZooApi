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
                options.Configuration = configuration.GetConnectionString("Redis");
                options.InstanceName = "ZooApi";
            });
            
            services.AddMassTransit(x =>
            {
                x.AddConsumers(typeof(AnimalCreatedConsumer).Assembly);
                
                /*x.AddEntityFrameworkOutbox<ZooDbContext>(o =>
                {
                    o.QueryDelay = TimeSpan.FromSeconds(10);
                    o.UsePostgres();
                    o.UseBusOutbox();
                });*/
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("localhost", 5672,"/", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });
                    cfg.UseMessageRetry(r  => r.Interval(3, TimeSpan.FromSeconds(5)));
                    cfg.ConfigureEndpoints(context); 
                });
            });
            
            services.AddScoped<IAnimalRepository, AnimalRepository>();
            
            return services;
        }
    }