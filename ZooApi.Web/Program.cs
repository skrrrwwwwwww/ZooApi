using ZooApi.Application.Extensions;
using ZooApi.Infrastructure.Extensions;
using ZooApi.Web.Endpoints;
using ZooApi.Web.ExceptionHandlers;
using ZooApi.Web.MiddlewareExtensions;
using ZooApi.Web.ServiceExtensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddWebServices();

var app = builder.Build();

app.UseApiPipeline();

app.Run();