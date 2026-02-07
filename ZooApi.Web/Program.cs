using ZooApi.Application.Extensions;
using ZooApi.Infrastructure.Extensions;
using ZooApi.Web.ExceptionHandlers;
using ZooApi.Web.MiddlewareExtensions;

var builder = WebApplication.CreateBuilder(args);

builder.Host.RegisterSerilog(); 

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddOpenApi(); 
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseExceptionHandler();
app.UseCustomLogging();
app.MapControllers();
app.UseScalarAlways();
app.Run();