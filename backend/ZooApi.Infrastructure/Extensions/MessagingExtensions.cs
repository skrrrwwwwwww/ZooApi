namespace ZooApi.Infrastructure.Extensions;

public static class MessagingExtensions
{
    public static IServiceCollection AddMessaging(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumers(typeof(AnimalCreatedConsumer).Assembly);

            x.AddEntityFrameworkOutbox<ZooDbContext>(o =>
            {
                o.UsePostgres();
                o.UseBusOutbox();
                o.QueryDelay = TimeSpan.FromSeconds(10);
            });

            x.UsingRabbitMq((context, cfg) =>
            {
                var rabbitSettings = configuration.GetSection("RabbitMq");
                var username = rabbitSettings["Username"];
                var password = rabbitSettings["Password"];
                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                {
                    throw new InvalidOperationException(
                        "RabbitMq:Username and RabbitMq:Password must be configured.");
                }

                cfg.Host(rabbitSettings["Host"] ?? "localhost", h =>
                {
                    h.Username(username);
                    h.Password(password);
                });
                cfg.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));
                cfg.ConfigureEndpoints(context, new KebabCaseEndpointNameFormatter("ZooApi", false));
            });
        });

        return services;
    }
}