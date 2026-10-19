namespace ZooApi.Web.ServiceExtensions;

public static class PipelineExtensions
{
    public static WebApplication UseApiPipeline(this WebApplication app)
    {
        app.UseExceptionHandler();

        app.UseCustomLogging();

        app.UseCors("FrontendCorsPolicy");

        if (!app.Environment.IsDevelopment())
            app.UseHttpsRedirection();

        app.UseSwaggerDocumentation();
        app.MapHealthChecks("/health");
        app.MapOpenApi();

        app.MapControllers();

        return app;
    }
}