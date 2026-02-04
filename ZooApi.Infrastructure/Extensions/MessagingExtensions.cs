    using MassTransit;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using ZooApi.Application.Services;
    
    namespace ZooApi.Infrastructure.Extensions;

    public static class MessagingExtensions
    {
        public static IServiceCollection AddMessaging(this IServiceCollection services, IConfiguration configuration)
        {
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
                    cfg.ConfigureEndpoints(context, new KebabCaseEndpointNameFormatter("ZooApi", false)); 
                });
            });
            
            return services;
        }
    }