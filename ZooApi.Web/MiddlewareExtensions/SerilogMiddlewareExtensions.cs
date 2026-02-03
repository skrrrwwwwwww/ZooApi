using Serilog;

namespace ZooApi.Web.MiddlewareExtensions;

public static class SerilogMiddlewareExtensions
{
    public static IApplicationBuilder UseCustomSerilogRequestLogging(this IApplicationBuilder app) {
        return app.UseSerilogRequestLogging(opts => {
            opts.EnrichDiagnosticContext = (diagnosticContext, httpContext) => {
                diagnosticContext.Set("UserId", httpContext.User?.Identity?.Name ?? "-");
                diagnosticContext.Set("RequestPath", httpContext.Request.Path);
            };
        });
    }
}