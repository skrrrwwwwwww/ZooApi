namespace ZooApi.Web.ServiceExtensions;

public static class ServiceExtensions
{
    public static void AddWebServices(this WebApplicationBuilder builder)
    {
        builder.Host.RegisterSerilog(); 
        
        builder.Services.AddApplication();
        builder.Services.AddInfrastructure(builder.Configuration);
        builder.Services.AddOpenApi(); 
        builder.Services.AddProblemDetails();
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
    }
}
