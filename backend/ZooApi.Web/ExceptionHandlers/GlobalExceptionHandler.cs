namespace ZooApi.Web.ExceptionHandlers;

using ZooApi.Domain.Exceptions;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
    CancellationToken cancellationToken)
    {
        logger.LogError(exception, "Error: {Message}", exception.Message);

        var (statusCode, title) = exception switch
        {
            KeyNotFoundException => (StatusCodes.Status404NotFound, "Not Found"),
            ArgumentException => (StatusCodes.Status400BadRequest, "Invalid Request"),
            AnimalTooTiredException => (StatusCodes.Status409Conflict, "Conflict"),
            _ => (StatusCodes.Status500InternalServerError, "Server Error")
        };

        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = statusCode < StatusCodes.Status500InternalServerError
                ? exception.Message
                : "An unexpected error occurred."
        }, cancellationToken);

        return true;
    }
}