using FluentResults;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Shop.Common.Errors;

namespace Shop.Common.Mediator;

public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!validators.Any())
            return await next();

        var context = new ValidationContext<TRequest>(request);

        var failures = (await Task.WhenAll(
            validators.Select(v => v.ValidateAsync(context, cancellationToken))
        ))
        .SelectMany(r => r.Errors)
        .Where(f => f is not null)
        .ToList();

        if (failures.Any())
        {
            return HandleValidationFailure(failures);
        }

        return await next();
    }

    private static TResponse HandleValidationFailure(List<ValidationFailure> failures)
    {
        if (typeof(TResponse) == typeof(Result))
        {
            // Para Result não genérico
            var result = ValidationError.Fail(failures);
            return (TResponse)(object)result;
        }
        else if (IsResultOfT<TResponse>())
        {
            // Para Result<T> genérico
            var resultType = typeof(TResponse).GetGenericArguments()[0];

            // Usar o método genérico ValidationError.Fail<T> que criamos
            var failMethod = typeof(ValidationError)
                .GetMethod(nameof(ValidationError.Fail),
                    genericParameterCount: 1,
                    types: new[] { typeof(IEnumerable<ValidationFailure>) });

            if (failMethod != null)
            {
                var genericMethod = failMethod.MakeGenericMethod(resultType);
                var result = genericMethod.Invoke(null, [failures]);
                return (TResponse)result!;
            }
        }

        throw new ValidationException(failures);
    }

    private static bool IsResultOfT<TResponse>()
    {
        return typeof(TResponse).IsGenericType &&
               typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>);
    }
}