using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using ZooApi.Web.MiddlewareExtensions;

namespace ZooApi.Tests;

public class OpenApiSecurityTests
{
    [Fact]
    public async Task OpenApiDocument_DescribesBearerAuthentication()
    {
        var builder = WebApplication.CreateBuilder();
        builder.WebHost.UseTestServer();
        builder.Services.AddSwaggerDocumentation();

        await using var app = builder.Build();
        app.MapOpenApi();
        app.MapGet("/api/animals", () => Results.Ok());
        await app.StartAsync();

        var response = await app.GetTestClient().GetAsync("/openapi/v1.json");
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

        using var document = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
        var bearerScheme = document.RootElement
            .GetProperty("components")
            .GetProperty("securitySchemes")
            .GetProperty("Bearer");

        Assert.Equal("http", bearerScheme.GetProperty("type").GetString());
        Assert.Equal("bearer", bearerScheme.GetProperty("scheme").GetString());

        var operation = document.RootElement
            .GetProperty("paths")
            .GetProperty("/api/animals")
            .GetProperty("get");
        Assert.Equal("Bearer", operation.GetProperty("security")[0].EnumerateObject().Single().Name);
    }
}
