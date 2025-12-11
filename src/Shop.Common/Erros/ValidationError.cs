using FluentResults;
using FluentValidation.Results;

namespace Shop.Common.Errors;

public class ValidationError : Error
{
    public ValidationError(string message) : base(message)
    {
        Metadata.Add("ErrorType", "ValidationError");
    }

    public ValidationError(ValidationFailure failure) : base(failure.ErrorMessage)
    {
        Metadata.Add("ErrorType", "ValidationError");
        Metadata.Add("PropertyName", failure.PropertyName);
        Metadata.Add("AttemptedValue", failure.AttemptedValue);
    }

    public static Result Fail(string message)
    {
        return Result.Fail(new ValidationError(message));
    }

    public static Result Fail(IEnumerable<string> messages)
    {
        var errors = messages.Select(msg => new ValidationError(msg));
        return Result.Fail(errors);
    }

    public static Result Fail(IEnumerable<ValidationFailure> failures)
    {
        var errors = failures.Select(f => new ValidationError(f));
        return Result.Fail(errors);
    }

    public static Result<T> Fail<T>(string message)
    {
        return Result.Fail<T>(new ValidationError(message));
    }

    public static Result<T> Fail<T>(IEnumerable<string> messages)
    {
        var errors = messages.Select(msg => new ValidationError(msg));
        return Result.Fail<T>(errors);
    }

    public static Result<T> Fail<T>(IEnumerable<ValidationFailure> failures)
    {
        var errors = failures.Select(f => new ValidationError(f));
        return Result.Fail<T>(errors);
    }
}