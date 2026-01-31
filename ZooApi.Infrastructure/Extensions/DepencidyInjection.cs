    using MassTransit;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using ZooApi.Application.Services;

    namespace ZooApi.Infrastructure.Extensions;

    public static class DepencidyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
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
            return services;
        }
    }