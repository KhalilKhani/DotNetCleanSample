using static CleanSample.Domain.ValueObjects.DateOfBirth.ErrorMessage;

namespace CleanSample.UnitTests.Domain.ValueObjects;

public class DateOfBirthTests
{
    public static IEnumerable<object?[]> InvalidData =>
        new List<object?[]>
        {
            new[] { DateTime.Today, new ValidationError(ErrorCodes.InvalidBirthDate, PastDate).Message },
            new[] { DateTime.Today.AddDays(1), new ValidationError(ErrorCodes.InvalidBirthDate, PastDate).Message },
        };

    public static IEnumerable<object?[]> ValidData =>
        new List<object?[]>
        {
            new object?[] { DateTime.Today.AddDays(-1) },
            new object?[] { new DateTime(2000, 11, 21) },
        };

    [Theory]
    [MemberData(nameof(InvalidData))]
    public void Create_InvalidDateOfBirth_ReturnsFailure(DateTime dateTime, Error expectedError)
    {
        // Act
        var result = DateOfBirth.Create(dateTime);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Message.Should().Be(expectedError);
    }


    [Theory]
    [MemberData(nameof(ValidData))]
    public void Create_ValidDateOfBirth_ReturnsSuccess(DateTime dateTime)
    {
        // Act
        var result = DateOfBirth.Create(dateTime);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be(dateTime);
    }
}