using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using ZooApi.Domain.Exceptions;
using ZooApi.Web.ExceptionHandlers;

namespace ZooApi.Tests;

public class GlobalExceptionHandlerTests
{
    [Theory]
    [InlineData(typeof(ArgumentOutOfRangeException), StatusCodes.Status400BadRequest)]
    [InlineData(typeof(AnimalTooTiredException), StatusCodes.Status409Conflict)]
    public async Task DomainErrors_AreReturnedAsClientErrors(Type exceptionType, int expectedStatus)
    {
        Exception exception = exceptionType == typeof(ArgumentOutOfRangeException)
            ? new ArgumentOutOfRangeException("value")
            : new AnimalTooTiredException();
        var context = new DefaultHttpContext();
        await using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        var handler = new GlobalExceptionHandler(NullLogger<GlobalExceptionHandler>.Instance);
        var handled = await handler.TryHandleAsync(context, exception, CancellationToken.None);

        Assert.True(handled);
        Assert.Equal(expectedStatus, context.Response.StatusCode);
    }

    [Fact]
    public async Task UnexpectedErrors_DoNotExposeExceptionDetails()
    {
        var context = new DefaultHttpContext();
        await using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        var handler = new GlobalExceptionHandler(NullLogger<GlobalExceptionHandler>.Instance);
        await handler.TryHandleAsync(
            context,
            new InvalidOperationException("sensitive internal detail"),
            CancellationToken.None);

        responseBody.Position = 0;
        var problem = await JsonSerializer.DeserializeAsync<ProblemDetails>(
            responseBody,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.Equal(StatusCodes.Status500InternalServerError, context.Response.StatusCode);
        Assert.Equal("An unexpected error occurred.", problem?.Detail);
        Assert.DoesNotContain("sensitive internal detail", problem?.Detail);
    }
}
