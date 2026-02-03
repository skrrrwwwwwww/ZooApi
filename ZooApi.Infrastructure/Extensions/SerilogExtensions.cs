using Microsoft.AspNetCore.Builder;
using Serilog;

namespace ZooApi.Infrastructure.Extensions;

public static class SerilogExtensions
{
    public static void RegisterSerilog(this ConfigureHostBuilder host)
    {
        host.UseSerilog((context, config) => config
            .ReadFrom.Configuration(context.Configuration)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File("Logs/api-.txt", 
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 7));
    }

    public static void UseCustomLogging(this IApplicationBuilder app)
    {
        app.UseSerilogRequestLogging(opts =>
        {
            opts.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
            {
                diagnosticContext.Set("UserId", httpContext.User?.Identity?.Name ?? "-");
                diagnosticContext.Set("RequestPath", httpContext.Request.Path);
            };
        });
    }
}