using Serilog;
using ZooApi.Application.Extensions;
using ZooApi.Infrastructure.Extensions;
using ZooApi.Web.ExceptionHandlers;

var builder = WebApplication.CreateBuilder(args);

builder.Host.RegisterSerilog(); 

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseExceptionHandler();
app.UseSerilogRequestLogging(opts =>
{
    opts.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
    {
        diagnosticContext.Set("UserId", httpContext.User?.Identity?.Name ?? "-");
        diagnosticContext.Set("RequestPath", httpContext.Request.Path);
    };
});

app.MapControllers();
app.Run();