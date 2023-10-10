using CleanSample.Domain.Abstractions.ValueObject;
using CleanSample.Domain.Enums;
using CleanSample.Utility;

namespace CleanSample.Domain.ValueObjects;

public record PhoneNumber : ValueObject
{
    public static class ErrorMessage
    {
        public const string InvalidMobileNumber = "Invalid Mobile number";
    }

    public string Value { get; init; }

    private PhoneNumber(string value)
    {
        Value = value;
    }

    public static Result<PhoneNumber> Create(string value)
    {
        Result<bool> validation = Validate(value);
        return validation.IsSuccess
            ? Result<PhoneNumber>.Success(new PhoneNumber(value))
            : Result<PhoneNumber>.Failure(validation.Error);
    }

    private static Result<bool> Validate(string value)
    {
        try
        {
            var phoneNumberUtil = PhoneNumberUtil.GetInstance();
            var number = phoneNumberUtil.Parse(value, null);
            if (number == null || phoneNumberUtil.GetNumberType(number) != PhoneNumberType.MOBILE)
                return Failure(ErrorCodes.InvalidMobileNumber, ErrorMessage.InvalidMobileNumber);
        }
        catch (NumberParseException)
        {
            return Failure(ErrorCodes.InvalidMobileNumber, ErrorMessage.InvalidMobileNumber);
        }
        
        return true;
    }
}