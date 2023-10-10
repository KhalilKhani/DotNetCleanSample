using CleanSample.Domain.Abstractions.ValueObject;
using CleanSample.Domain.Enums;
using CleanSample.Utility;

namespace CleanSample.Domain.ValueObjects;

public record FirstName : ValueObject
{
    public static class ErrorMessages
    {
        public const string Required = $"{nameof(FirstName)} is required.";
        public const string LengthRange = $"{nameof(FirstName)} should be between 3 and 50 characters.";
        public const string LetterOnly = $"{nameof(FirstName)} should only contain letters.";
    }

    public string Value { get; init; }

    private FirstName(string value)
    {
        Value = value;
    }


    public static Result<FirstName> Create(string firstname)
    {
        var validation = Validate(firstname);
        return validation.IsSuccess
            ? Result<FirstName>.Success(new FirstName(firstname))
            : Result<FirstName>.Failure(validation.Error);
    }

    private static Result<bool> Validate(string firstname)
    {
        if (string.IsNullOrWhiteSpace(firstname))
            return Failure(ErrorCodes.InvalidFirstName, string.Format(ErrorMessages.Required, nameof(Value)));
        if (firstname.Length is < 3 or > 50)
            return Failure(ErrorCodes.InvalidFirstName, string.Format(ErrorMessages.LengthRange, nameof(Value)));
        if (!LettersOnly(firstname))
            return Failure(ErrorCodes.InvalidFirstName, string.Format(ErrorMessages.LetterOnly, nameof(Value)));

        return true;
    }

    private static bool LettersOnly(string str) => str.All(char.IsLetter);

    public override string ToString() => Value;
}