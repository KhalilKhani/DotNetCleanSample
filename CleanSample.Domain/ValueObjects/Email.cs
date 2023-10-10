using CleanSample.Domain.Abstractions.ValueObject;
using CleanSample.Domain.Enums;
using CleanSample.Utility;

namespace CleanSample.Domain.ValueObjects;

public record Email : ValueObject
{
    public static class ErrorMessage
    {
        public const string EmailRequired = $"{nameof(Email)} is required.";
        public const string InvalidEmailFormat = "Invalid email format.";
    }

    public string Value { get; init; }

    private Email(string value)
    {
        Value = value;
    }

    public static Result<Email> Create(string value)
    {
        Result<bool> validation = Validate(value);
        return validation.IsSuccess
            ? Result<Email>.Success(new Email(value))
            : Result<Email>.Failure(validation.Error);
    }

    private static Result<bool> Validate(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Failure(ErrorCodes.InvalidEmailAddress, ErrorMessage.EmailRequired);

        if (!IsValidFormat(value))
            return Failure(ErrorCodes.InvalidEmailAddress, ErrorMessage.InvalidEmailFormat);

        return true;
    }

    private static bool IsValidFormat(string value)
    {
        try
        {
            MailAddress mailAddress = new(value);
            return mailAddress.Address == value && mailAddress.Host.Contains('.');
        }
        catch
        {
            return false;
        }
    }
}