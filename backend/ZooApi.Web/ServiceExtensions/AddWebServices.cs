namespace ZooApi.Web.ServiceExtensions;

public static class ServiceExtensions
{
    public static void AddWebServices(this WebApplicationBuilder builder)
    {
        builder.Host.RegisterSerilog(); 
        builder.Services.AddApplication();
        builder.Services.AddInfrastructure(builder.Configuration);
        builder.Services.AddProblemDetails();
        builder.Services.AddSwaggerDocumentation(); 
        builder.Services.AddControllers(options => options.Filters.Add<AsyncValidationFilter>());
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        // Liveness-эндпоинт для compose healthcheck. Внешние зависимости сюда не
        // включаем осознанно: доступность postgres и rabbitmq compose уже проверяет
        // через depends_on, а сбой брокера ронял бы вполне рабочий API.
        builder.Services.AddHealthChecks();
    }
}
