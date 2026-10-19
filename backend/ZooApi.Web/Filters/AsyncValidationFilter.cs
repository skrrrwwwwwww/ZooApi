using System.Collections.Concurrent;
using System.Reflection;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ZooApi.Web.Filters;

// ModelState-пайплайн ASP.NET синхронен, поэтому валидаторы с MustAsync
// (проверки по базе) через AddFluentValidationAutoValidation() падают с 500.
// Валидируем асинхронно до вызова экшна.
public sealed class AsyncValidationFilter(IServiceProvider serviceProvider) : IAsyncActionFilter
{
    private static readonly MethodInfo ValidateMethod = typeof(AsyncValidationFilter)
        .GetMethod(nameof(ValidateAsync), BindingFlags.NonPublic | BindingFlags.Static)!;

    private static readonly ConcurrentDictionary<Type, MethodInfo> ClosedValidateMethods = new();

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var failures = new List<ValidationFailure>();

        foreach (var argument in context.ActionArguments.Values)
        {
            if (argument is null)
                continue;

            var argumentType = argument.GetType();
            var validatorType = typeof(IValidator<>).MakeGenericType(argumentType);
            var closedMethod = ClosedValidateMethods.GetOrAdd(
                argumentType, type => ValidateMethod.MakeGenericMethod(type));

            foreach (var validator in serviceProvider.GetServices(validatorType))
            {
                var task = (Task<IEnumerable<ValidationFailure>>)closedMethod
                    .Invoke(null, [validator, argument, CancellationToken.None])!;

                failures.AddRange(await task);
            }
        }

        if (failures.Count == 0)
        {
            await next();
            return;
        }

        var modelState = new ModelStateDictionary();

        foreach (var failure in failures)
        {
            if (!modelState.ContainsKey(failure.PropertyName))
                modelState.AddModelError(failure.PropertyName, failure.ErrorMessage);
        }

        context.Result = new BadRequestObjectResult(new ValidationProblemDetails(modelState));
    }

    private static async Task<IEnumerable<ValidationFailure>> ValidateAsync<T>(
        IValidator<T> validator, T instance, CancellationToken cancellationToken)
        => (await validator.ValidateAsync(instance, cancellationToken)).Errors;
}
