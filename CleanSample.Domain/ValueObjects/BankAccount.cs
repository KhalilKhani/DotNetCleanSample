using CleanSample.Domain.Abstractions.ValueObject;
using CleanSample.Domain.Enums;
using CleanSample.Utility;

namespace CleanSample.Domain.ValueObjects;

public record BankAccount : ValueObject
{
    public static class ErrorMessage
    {
        public const string NumberRequired = $"{nameof(BankAccount)} is required.";
        public const string InvalidBankAccountNumber = "Invalid Bank account number";
    }

    public string Number { get; init; }

    private BankAccount(string number)
    {
        Number = number;
    }

    public static Result<BankAccount> Create(string number)
    {
        Result<bool> validation = Validate(number);
        return validation.IsSuccess
            ? Result<BankAccount>.Success(new BankAccount(number))
            : Result<BankAccount>.Failure(validation.Error);
    }

    private static Result<bool> Validate(string number)
    {
        if (string.IsNullOrWhiteSpace(number))
            return Failure(ErrorCodes.InvalidBankAccountNumber, ErrorMessage.NumberRequired);
        
        if (!IsValidIban(number))
            return Failure(ErrorCodes.InvalidBankAccountNumber, ErrorMessage.InvalidBankAccountNumber);

        return true;
    }

    private static bool IsValidIban(string iban)
    {
        var regex = new Regex(@"^([A-Z]{2}[0-9]{2})([A-Z0-9]{4,32})$");
        return regex.IsMatch(iban);
    }
}