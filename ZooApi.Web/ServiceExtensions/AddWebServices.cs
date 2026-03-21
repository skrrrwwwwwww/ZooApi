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
        builder.Services.AddControllers();
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
    }
}
