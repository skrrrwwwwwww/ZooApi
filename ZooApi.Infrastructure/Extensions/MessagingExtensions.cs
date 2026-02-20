using ZooApi.Infrastructure.Messaging;

namespace ZooApi.Infrastructure.Extensions;

public static class MessagingExtensions
{
    public static IServiceCollection AddMessaging(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<LogReceiveObserver>();
        services.AddQuartz(q => { q.UseMicrosoftDependencyInjectionJobFactory(); });
        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
        services.AddMassTransit(x =>
        {
            x.AddConsumers(typeof(AnimalCreatedConsumer).Assembly);
            x.AddEntityFrameworkOutbox<ZooDbContext>(o => 
            {
                o.UsePostgres();
                o.UseBusOutbox();
                o.QueryDelay = TimeSpan.FromSeconds(1);
            });
            
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.ConnectReceiveObserver(context.GetRequiredService<LogReceiveObserver>());
                
                var rabbitSettings = configuration.GetSection("RabbitMq");
                cfg.Host(rabbitSettings["Host"] ?? "localhost", h =>
                {
                    h.Username(rabbitSettings["Username"] ?? "guest");
                    h.Password(rabbitSettings["Password"] ?? "guest");
                });
                cfg.UseMessageRetry(r  => r.Exponential(5, TimeSpan.FromSeconds(1), TimeSpan.FromMinutes(1), TimeSpan.FromSeconds(5)));
                cfg.ConfigureEndpoints(context, new KebabCaseEndpointNameFormatter("ZooApi", false)); 
            });
        });
        
        return services;
    }
}