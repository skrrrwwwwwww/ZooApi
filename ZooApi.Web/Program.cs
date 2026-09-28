AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var builder = WebApplication.CreateBuilder(args);
builder.AddWebServices();

var app = builder.Build();

await app.MigrateDatabaseAsync();

app.UseApiPipeline();   

app.Run();