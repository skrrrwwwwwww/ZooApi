var builder = WebApplication.CreateBuilder(args);
builder.AddWebServices();

var app = builder.Build();
app.UseApiPipeline();

await app.RunAsync();
