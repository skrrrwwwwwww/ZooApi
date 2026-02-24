namespace ZooApi.Web.Filters;

public class ValidationFilter<T>(IValidator<T> validator) : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var arg = context.Arguments.FirstOrDefault(x => x is T);

        if (arg is T dto)
        {
            var result = await validator.ValidateAsync(dto);
            if (!result.IsValid) return Results.ValidationProblem(result.ToDictionary());
        }
        
        return await next(context);
    }
}