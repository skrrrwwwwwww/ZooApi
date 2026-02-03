using Scalar.AspNetCore;

namespace ZooApi.Web.MiddlewareExtensions;

public static class ScalarMiddlewareExtensions
{
    public static void UseScalarAlways(this IEndpointRouteBuilder app)
    {
        app.MapOpenApi(); 
        
        app.MapScalarApiReference("/scalar", options => 
        {
            options.WithTitle("Zoo API Reference")
                .WithTheme(ScalarTheme.DeepSpace)
                .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
        });
        
        app.MapGet("/", () => Results.Redirect("/scalar")).ExcludeFromDescription();
    }
}