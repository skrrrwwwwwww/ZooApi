namespace ZooApi.Application.Extensions;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAnimalService, AnimalService>();
        services.AddScoped<IRedisCacheService, RedisCacheService>();
        services.AddScoped<IEmailService, EmailService>();
        
        services.AddScoped<IValidator<CreateAnimalDto>, CreateAnimalDtoValidator>();
        services.AddScoped<IValidator<FeedDto>, FeedDtoValidator>();
        services.AddScoped<IValidator<PlayDto>, PlayWithAnimalRequestValidator>();
        
        services.AddSingleton<AnimalMap>();
        
        return services;
    }
}