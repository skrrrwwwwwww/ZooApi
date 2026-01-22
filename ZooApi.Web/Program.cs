using Microsoft.EntityFrameworkCore;
using ZooApi.Application.Interfaces;
using ZooApi.Application.Services;
using ZooApi.Infrastructure.Data;
using Serilog;
using ZooApi.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, config) => config
    .ReadFrom.Configuration(context.Configuration)
    .WriteTo.Console()
    .WriteTo.File("logs/api-.txt", 
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 7));

builder.Services.AddDbContext<ZooDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")
                      ?? "Host=localhost;Database=postgres;Username=postgres;Password=password123"));


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

app.UseHttpsRedirection();

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