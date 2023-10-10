using static CleanSample.Domain.ValueObjects.Email.ErrorMessage;

namespace CleanSample.UnitTests.Domain.ValueObjects;

public class EmailTests
{

    public static IEnumerable<object?[]> InvalidData =>
        new List<object?[]>
        {
            new[] { null, new ValidationError(ErrorCodes.InvalidEmailAddress, EmailRequired).Message },
            new[] { "", new ValidationError(ErrorCodes.InvalidEmailAddress, EmailRequired).Message },
            new[] { "not-an-email", new ValidationError(ErrorCodes.InvalidEmailAddress, InvalidEmailFormat).Message },
            new[] { "missing@domain", new ValidationError(ErrorCodes.InvalidEmailAddress, InvalidEmailFormat).Message }
        };

    public static IEnumerable<object?[]> ValidData =>
        new List<object?[]>
        {
            new object ?[] { "khalil.khani2020@gmail.com" },
            new object ?[] { "will.smith@example.com" },
            new object ?[] { "jane.doe@example.org" }
        };

    [Theory]
    [MemberData(nameof(ValidData))]
    public void Create_ShouldReturnSuccess_WhenEmailIsValid(string email)
    {
        // Act
        var result = Email.Create(email);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be(email);
    }


    [Theory]
    [MemberData(nameof(InvalidData))]
    public void Create_ShouldReturnFailure_WhenEmailIsInvalid(string email, Error expectedError)
    {
        // Act
        var result = Email.Create(email);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Message.Should().Be(expectedError);
    }
}