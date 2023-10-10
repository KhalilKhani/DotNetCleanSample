using CleanSample.Domain.Abstractions.ValueObject;
using CleanSample.Domain.Enums;
using CleanSample.Utility;

namespace CleanSample.Domain.ValueObjects;

public record LastName : ValueObject
{
    public static class ErrorMessages
    {
        public const string Required = $"{nameof(LastName)} is required.";
        public const string LengthRange = $"{nameof(LastName)} should be between 3 and 50 characters.";
        public const string LetterOnly = $"{nameof(LastName)} should only contain letters.";
    }

    public string Value { get; init; }

    private LastName(string value)
    {
        Value = value;
    }


    public static Result<LastName> Create(string lastname)
    {
        var validation = Validate(lastname);
        return validation.IsSuccess
            ? Result<LastName>.Success(new LastName(lastname))
            : Result<LastName>.Failure(validation.Error);
    }

    private static Result<bool> Validate(string lastname)
    {
        if (string.IsNullOrWhiteSpace(lastname))
            return Failure(ErrorCodes.InvalidLastName, ErrorMessages.Required);
        if (lastname.Length is < 3 or > 50)
            return Failure(ErrorCodes.InvalidLastName, ErrorMessages.LengthRange);
        if (!LettersOnly(lastname))
            return Failure(ErrorCodes.InvalidLastName, ErrorMessages.LetterOnly);

        return true;
    }

    private static bool LettersOnly(string str) => str.All(char.IsLetter);

    public override string ToString() => Value;
}