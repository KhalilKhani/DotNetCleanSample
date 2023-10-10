using static CleanSample.Domain.ValueObjects.LastName.ErrorMessages;

namespace CleanSample.UnitTests.Domain.ValueObjects;

public class LastNameTests
{
    public static IEnumerable<object?[]> InvalidNames =>
        new List<object?[]>
        {
            new[] { null, new ValidationError(ErrorCodes.InvalidLastName, Required).Message },
            new[] { "", new ValidationError(ErrorCodes.InvalidLastName, Required).Message },
            new[] { new string('a', 2), new ValidationError(ErrorCodes.InvalidLastName, LengthRange).Message },
            new[] { new string('a', 51), new ValidationError(ErrorCodes.InvalidLastName, LengthRange).Message },
            new[] { "Lastname1", new ValidationError(ErrorCodes.InvalidLastName, LetterOnly).Message }
        };

    public static IEnumerable<object?[]> ValidNames =>
        new List<object?[]>
        {
            new object ?[] {"Lastname" },
            new object ?[] {new string('B', 3) },
            new object ?[] {new string('B', 50) },
        };
        

    [Theory]
    [MemberData(nameof(ValidNames))]
    public void FullName_Create_ShouldReturnSuccess_WhenNamesAreValid(string lastname)
    {
        // Act
        var result = LastName.Create(lastname);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be(lastname);
    }


    [Theory]
    [MemberData(nameof(InvalidNames))]
    public void FullName_Create_ShouldReturnFailure_WhenNamesAreInvalid(string lastname, Error expectedError)
    {
        // Act
        var result = LastName.Create(lastname);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Message.Should().Be(expectedError);
    }
}