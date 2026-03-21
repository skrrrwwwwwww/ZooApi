    namespace ZooApi.Web.ServiceExtensions;

    public static class PipelineExtensions
    {
        public static WebApplication UseApiPipeline(this WebApplication app)
        {
            if (!app.Environment.IsDevelopment()) 
                app.UseHttpsRedirection();
            
            app.UseExceptionHandler();
            app.UseCustomLogging();
            app.MapOpenApi();
            app.UseSwaggerAlways();
            app.MapControllers();

            return app;
        }
    }