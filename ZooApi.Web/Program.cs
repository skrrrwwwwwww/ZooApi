using Microsoft.EntityFrameworkCore;
using ZooApi.Application.Interfaces;
using ZooApi.Application.Services;
using ZooApi.Infrastructure;
using Serilog;
using ZooApi.Infrastructure;
using ZooApi.Infrastructure.Repositories;
using ZooApi.Web.ExceptionHandlers;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, config) => config
    .ReadFrom.Configuration(context.Configuration)
    .WriteTo.Console()
    .WriteTo.File("logs/api-.txt", 
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 7));

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Строка подключения 'DefaultConnection' не найдена в конфиге!");
}

builder.Services.AddDbContext<ZooDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")
                      ?? "Host=localhost;Database=postgres;Username=postgres;Password=password123"));


builder.Services.AddControllers();
builder.Services.AddScoped<IAnimalRepository, AnimalRepository>();
builder.Services.AddScoped<IAnimalService, AnimalService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

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