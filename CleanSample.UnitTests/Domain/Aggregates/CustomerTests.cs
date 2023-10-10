namespace CleanSample.UnitTests.Domain.Aggregates;

public class CustomerTests
{
    private const string FirstName = "Khalil";
    private const string LastName = "Khani";
    private static readonly DateTime Dob = new(1990, 1, 1);
    private const string PhoneNumber = "+989145799298";
    private const string Email = "Khalil.Khani@example.com";
    private const string AccountNumber = "IR000000000000000000000001";

    [Fact]
    public void Create_ShouldSucceedWithValidData()
    {
        // Act
        var result = Customer.Create(FirstName, LastName, Dob, PhoneNumber, Email, AccountNumber);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.DomainEvents.Should().ContainSingle()
            .Which.Should().BeOfType<CustomerCreatedDomainEvent>();
    }

    [Fact]
    public void Update_ShouldSucceedWithValidData()
    {
        // Arrange
        var customer = Customer.Create(FirstName, LastName, Dob, PhoneNumber, Email, AccountNumber).Value;
        const string newFirstName = "John";
        const string newLastName = "Smith";
        DateTime newDob = new(1995, 1, 1);
        const string newPhoneNumber = "+989370033315";
        const string newEmail = "John.smith@example.com";
        const string newAccountNumber = "IR000000000000000000000002";

        // Act
        var result =
            customer.Update(newFirstName, newLastName, newDob, newPhoneNumber, newEmail, newAccountNumber);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.DomainEvents.Should().HaveCount(2); // One for creation, one for update
        result.Value.DomainEvents.Last().Should().BeOfType<CustomerUpdatedDomainEvent>();
    }

    [Fact]
    public void MarkAsDeleted_ShouldRaiseCustomerDeletedDomainEvent()
    {
        // Arrange
        var customer = Customer.Create(FirstName, LastName, Dob, PhoneNumber, Email, AccountNumber).Value;

        // Act
        customer.MarkAsDeleted();

        // Assert
        customer.DomainEvents.Should().HaveCount(2); // One for creation, one for deletion
        customer.DomainEvents.Last().Should().BeOfType<CustomerDeletedDomainEvent>();
    }
}