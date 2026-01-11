using ZooApi.Application.Interfaces;
using ZooApi.Application.Services;
using ZooApi.Infrastructure.Data;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, config) => config
    .ReadFrom.Configuration(context.Configuration)
    .WriteTo.Console()
    .WriteTo.File("logs/api-.txt", 
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 7));

builder.Services.AddControllers();
builder.Services.AddScoped<IAnimalRepository, AnimalRepository>();
builder.Services.AddScoped<IAnimalService, AnimalService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging(opts =>
{
    opts.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
    {
        diagnosticContext.Set("UserId", httpContext.User?.Identity?.Name ?? "-");
        diagnosticContext.Set("RequestPath", httpContext.Request.Path);
    };
});

app.MapControllers();
app.UseHttpsRedirection();
app.Run();