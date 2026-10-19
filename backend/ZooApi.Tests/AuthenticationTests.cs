using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Security.Claims;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using ZooApi.Web.ServiceExtensions;
using ZooApi.Web.Controllers;

namespace ZooApi.Tests;

public class AuthenticationTests
{
    private const string Issuer = "https://identity.example.test/";
    private const string Audience = "zoo-api";

    [Fact]
    public async Task ProtectedEndpointsRequireValidExternalJwtAndHealthIsPublic()
    {
        using var rsa = RSA.Create(2048);
        var signingKey = new RsaSecurityKey(rsa);
        var openIdConfiguration = new OpenIdConnectConfiguration { Issuer = Issuer };
        openIdConfiguration.SigningKeys.Add(signingKey);

        var builder = WebApplication.CreateBuilder();
        builder.WebHost.UseTestServer();
        builder.Configuration.AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["Authentication:Authority"] = Issuer,
            ["Authentication:Audience"] = Audience
        });
        builder.Services.AddApiAuthentication(builder.Configuration);
        builder.Services.PostConfigure<JwtBearerOptions>(
            JwtBearerDefaults.AuthenticationScheme,
            options => options.ConfigurationManager =
                new StaticConfigurationManager<OpenIdConnectConfiguration>(openIdConfiguration));

        await using var app = builder.Build();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapGet("/api/animals", () => Results.Ok()).RequireAuthorization();
        app.MapGet("/health", () => Results.Ok()).AllowAnonymous();
        await app.StartAsync();

        var client = app.GetTestClient();

        var missingTokenResponse = await client.GetAsync("/api/animals");
        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, missingTokenResponse.StatusCode);

        var healthResponse = await client.GetAsync("/health");
        Assert.Equal(System.Net.HttpStatusCode.OK, healthResponse.StatusCode);

        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "not-a-jwt");
        var invalidTokenResponse = await client.GetAsync("/api/animals");
        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, invalidTokenResponse.StatusCode);

        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Bearer", CreateToken(signingKey, "wrong-audience"));
        var wrongAudienceResponse = await client.GetAsync("/api/animals");
        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, wrongAudienceResponse.StatusCode);

        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Bearer", CreateToken(signingKey, Audience));
        var validTokenResponse = await client.GetAsync("/api/animals");
        Assert.Equal(System.Net.HttpStatusCode.OK, validTokenResponse.StatusCode);
    }

    [Fact]
    public void AnimalAndOwnerControllersRequireAuthorization()
    {
        Assert.NotNull(typeof(AnimalsController).GetCustomAttributes(typeof(AuthorizeAttribute), inherit: true).SingleOrDefault());
        Assert.NotNull(typeof(OwnersController).GetCustomAttributes(typeof(AuthorizeAttribute), inherit: true).SingleOrDefault());
    }

    [Fact]
    public void AuthenticationRegistration_RequiresHttpsAuthorityAndAudience()
    {
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Authentication:Authority"] = "http://identity.example.test/",
                ["Authentication:Audience"] = Audience
            })
            .Build();

        Assert.Throws<InvalidOperationException>(() =>
        {
            services.AddApiAuthentication(configuration);
        });
    }

    private static string CreateToken(SecurityKey key, string audience)
    {
        var token = new JwtSecurityToken(
            issuer: Issuer,
            audience: audience,
            claims: [new Claim("sub", "test-user")],
            notBefore: DateTime.UtcNow.AddMinutes(-1),
            expires: DateTime.UtcNow.AddMinutes(5),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.RsaSha256));

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
