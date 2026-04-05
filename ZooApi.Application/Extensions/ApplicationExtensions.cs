using FluentValidation.AspNetCore;

namespace ZooApi.Application.Extensions;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAnimalService, AnimalService>();
        services.AddScoped<IOwnerService, OwnerService>();
        services.AddScoped<IRedisCacheService, RedisCacheService>();
        services.AddScoped<IEmailService, EmailService>();
        
        services.AddAutoMapper(typeof(AnimalProfile).Assembly);
        services.AddValidatorsFromAssembly(typeof(ApplicationExtensions).Assembly);
        services.AddFluentValidationAutoValidation(); 
        
        return services;
    }
}