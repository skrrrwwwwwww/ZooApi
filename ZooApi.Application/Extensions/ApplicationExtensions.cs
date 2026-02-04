    using Microsoft.Extensions.DependencyInjection;
    using ZooApi.Application.Interfaces;
    using ZooApi.Application.Profiles;
    using ZooApi.Application.Services;

    namespace ZooApi.Application.Extensions;

    public static class ApplicationExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IAnimalService, AnimalService>();
            services.AddScoped<IRedisCacheService, RedisCacheService>();
            services.AddScoped<IEmailService, EmailService>();
            
            services.AddAutoMapper(typeof(AnimalProfile).Assembly);
            
            return services;
        }
    }