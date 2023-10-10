using CleanSample.Domain.Abstractions.ValueObject;
using CleanSample.Domain.Enums;
using CleanSample.Utility;

namespace CleanSample.Domain.ValueObjects;

public record DateOfBirth : ValueObject
{
    public static class ErrorMessage
    {
        public const string PastDate = $"{nameof(DateOfBirth)} should be in the past.";
    }

    public DateTime Value { get; init; }

    private DateOfBirth(DateTime value)
    {
        Value = value;
    }

    public static Result<DateOfBirth> Create(DateTime value)
    {
        Result<bool> validation = Validate(value);
        return validation.IsSuccess
            ? Result<DateOfBirth>.Success(new DateOfBirth(value.Date))
            : Result<DateOfBirth>.Failure(validation.Error);
    }

    private static Result<bool> Validate(DateTime value)
    {
        if (value >= DateTime.Today)
            return Failure(ErrorCodes.InvalidBirthDate, ErrorMessage.PastDate);

        return true;
    }
}