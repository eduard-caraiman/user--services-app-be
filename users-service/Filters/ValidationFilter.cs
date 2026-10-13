using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace users_service.Filters;

public class ValidationFilter<TRequest> : IAsyncActionFilter
{
    private readonly IValidator<TRequest> _validator;

    public ValidationFilter(IValidator<TRequest> validator)
    {
        _validator = validator;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var request = context.ActionArguments.Values
            .OfType<TRequest>()
            .FirstOrDefault();

        if (request is null)
        {
            await next();
            return;
        }

        var validationResult = await _validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .GroupBy(error => error.PropertyName)
                .ToDictionary(
                    group => group.Key,
                    group => group.Select(error =>
                        error.ErrorMessage).ToArray());

            context.Result = new BadRequestObjectResult(
                new ValidationProblemDetails(errors)
                {
                    Status = StatusCodes.Status400BadRequest
                });

            return;
        }

        await next();
    }
}