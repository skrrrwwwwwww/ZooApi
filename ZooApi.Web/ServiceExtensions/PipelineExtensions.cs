namespace ZooApi.Web.ServiceExtensions;

public static class PipelineExtensions
{
    public static WebApplication UseApiPipeline(this WebApplication app)
    {
        app.UseHttpsRedirection();
        app.UseExceptionHandler();
        app.UseCustomLogging();
        app.UseScalarAlways();
        app.MapAnimals();
        

        return app;
    }
}