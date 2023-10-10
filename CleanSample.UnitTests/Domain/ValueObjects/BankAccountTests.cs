using static CleanSample.Domain.ValueObjects.BankAccount.ErrorMessage;

namespace CleanSample.UnitTests.Domain.ValueObjects;

public class BankAccountTests
{

    public static IEnumerable<object?[]> InvalidData =>
        new List<object?[]>
        {
            new[] { null, new ValidationError(ErrorCodes.InvalidBankAccountNumber, NumberRequired).Message },
            new[] { "", new ValidationError(ErrorCodes.InvalidBankAccountNumber, NumberRequired).Message },
            new[] { "12 34 56", new ValidationError(ErrorCodes.InvalidBankAccountNumber, InvalidBankAccountNumber).Message },
            new[] { "2500012541", new ValidationError(ErrorCodes.InvalidBankAccountNumber, InvalidBankAccountNumber).Message },
        };

    public static IEnumerable<object?[]> ValidData =>
        new List<object?[]>
        {
            new object ?[] { "IR000000000000000000000001"},
            new object ?[] { "IR000000000000000000000003" }
        };

    [Theory]
    [MemberData(nameof(ValidData))]
    public void Create_ShouldReturnSuccess_WhenNumberIsValid(string number)
    {
        // Act
        var result = BankAccount.Create(number);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Number.Should().Be(number);
    }


    [Theory]
    [MemberData(nameof(InvalidData))]
    public void Create_ShouldReturnFailure_WhenNumberIsInvalid(string number, Error expectedError)
    {
        // Act
        var result = BankAccount.Create(number);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Message.Should().Be(expectedError);
    }
}