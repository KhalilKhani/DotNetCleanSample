using static CleanSample.Domain.ValueObjects.FirstName.ErrorMessages;

namespace CleanSample.UnitTests.Domain.ValueObjects;

public class FullNameTests
{
    public static IEnumerable<object?[]> InvalidNames =>
        new List<object?[]>
        {
            new[] { null, new ValidationError(ErrorCodes.InvalidFirstName, Required).Message },
            new[] { new string('a', 2), new ValidationError(ErrorCodes.InvalidFirstName, LengthRange).Message },
            new[] { new string('a', 51), new ValidationError(ErrorCodes.InvalidFirstName, LengthRange).Message },
            new[] { "FirstName1", new ValidationError(ErrorCodes.InvalidFirstName, LetterOnly).Message },
        };

    public static IEnumerable<object?[]> ValidNames =>
        new List<object?[]>
        {
            new object ?[] { "Firstname" },
            new object ?[] { new string('A', 3)},
            new object ?[] { new string('A', 50)},
        };
        

    [Theory]
    [MemberData(nameof(ValidNames))]
    public void FullName_Create_ShouldReturnSuccess_WhenNamesAreValid(string firstname)
    {
        // Act
        var result = FirstName.Create(firstname);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be(firstname);
    }


    [Theory]
    [MemberData(nameof(InvalidNames))]
    public void FullName_Create_ShouldReturnFailure_WhenNamesAreInvalid(string firstname, Error expectedError)
    {
        // Act
        var result = FirstName.Create(firstname);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Message.Should().Be(expectedError);
    }
}