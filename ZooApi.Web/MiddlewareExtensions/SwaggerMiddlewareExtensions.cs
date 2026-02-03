namespace ZooApi.Web.MiddlewareExtensions;

public static class SwaggerMiddlewareExtensions
{
    public static void UseSwaggerAlways(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            options.RoutePrefix = string.Empty;
        });
    }
}