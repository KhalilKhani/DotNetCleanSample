using static CleanSample.Domain.ValueObjects.PhoneNumber.ErrorMessage;

namespace CleanSample.UnitTests.Domain.ValueObjects;

public class PhoneNumberTests
{
    public static IEnumerable<object?[]> InvalidData =>
        new List<object?[]>
        {
            new[] { null, new ValidationError(ErrorCodes.InvalidMobileNumber, InvalidMobileNumber).Message },
            new[] { "", new ValidationError(ErrorCodes.InvalidMobileNumber, InvalidMobileNumber).Message },
            new[] { "+442074601989", new ValidationError(ErrorCodes.InvalidMobileNumber, InvalidMobileNumber).Message }
        };

    public static IEnumerable<object?[]> ValidData =>
        new List<object?[]>
        {
            new object ?[] { "+989145799298" },
            new object ?[] { "+447123456789" }
        };


    [Theory]
    [MemberData(nameof(ValidData))]
    public void Create_ShouldReturnSuccess_WhenNumberIsValid(string number)
    {
        // Act
        var result = PhoneNumber.Create(number);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be(number);
    }

    [Theory]
    [MemberData(nameof(InvalidData))]
    public void Create_ShouldReturnFailure_WhenNumberIsInvalid(string number, Error expectedError)
    {
        // Act
        var result = PhoneNumber.Create(number);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Message.Should().Be(expectedError);
    }
}