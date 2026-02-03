using Microsoft.EntityFrameworkCore;
using ZooApi.Application.Extensions;
using ZooApi.Infrastructure;
using ZooApi.Infrastructure.Extensions;
using ZooApi.Web.ExceptionHandlers;
using ZooApi.Web.MiddlewareExtensions;

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
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.UseCustomSerilogRequestLogging();

app.UseExceptionHandler();

app.MapControllers();

app.ApplyMigrations();

app.Run();