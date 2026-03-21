namespace ZooApi.Web.MiddlewareExtensions;

public static class SwaggerMiddlewareExtensions
{
    public static void AddSwaggerDocumentation(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddOpenApi();
    }

    public static void UseSwaggerAlways(this WebApplication app)
    {
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/openapi/v1.json", "Zoo API v1");
            options.RoutePrefix = "swagger";
            options.DocumentTitle = "Zoo API Reference";
        });

        app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();
    }
}