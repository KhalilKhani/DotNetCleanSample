using CleanSample.Domain.Enums;
using CleanSample.Domain.Exceptions;
using CleanSample.Utility;

namespace CleanSample.Domain.Abstractions.ValueObject;

public abstract record ValueObject
{
    protected static Result<bool> Failure(ErrorCodes errorCode, string message)
    {
        return Result<bool>.Failure(new ValidationError(errorCode, message));
    }
}