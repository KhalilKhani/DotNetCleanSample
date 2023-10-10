namespace CleanSample.UnitTests.Infrastructure.Persistence.Repository;

public class CustomerRepositoryTests : IClassFixture<DatabaseFixture>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly CustomerRepository _repository;

    public CustomerRepositoryTests(DatabaseFixture fixture)
    {
        _dbContext = fixture.Context;
        _repository = new CustomerRepository(_dbContext);
    }

    [Fact]
    public async Task FetchCustomer_ShouldReturnCustomer_WhenExistingEmailIsGiven()
    {
        // Arrange
        var existingCustomer = CustomerFactory.Raymond();
        await _repository.CreateCustomer(existingCustomer.Value);

        // Act
        var result = await _repository.GetByEmail(existingCustomer.Value.Email.Value);
        result.IsSuccess.Should().BeTrue();
        result.Value.HasSameValuesAs(existingCustomer.Value, new List<string> { nameof(Customer.Id) })
            .Should().BeTrue();
    }

    [Fact]
    public async Task CreateCustomer_ShouldCreateNewCustomer_WhenUniqueCustomerIsGiven()
    {
        // Arrange
        var newCustomer = CustomerFactory.Khalil();

        // Act
        var result = await _repository.CreateCustomer(newCustomer.Value);

        // Assert
        result.IsSuccess.Should().BeTrue();
        var storedCustomer = await _dbContext.Set<Customer>().FirstOrDefaultAsync(c => c.Id == newCustomer.Value.Id);
        storedCustomer.Should().NotBeNull();
        storedCustomer.HasSameValuesAs(newCustomer.Value).Should().BeTrue();
    }

    [Fact]
    public async Task CreateCustomer_ShouldFailToCreateCustomer_WhenDuplicatedEmailOrPersonalInfoIsGiven()
    {
        var existingCustomer = CustomerFactory.Jane();
        await _repository.CreateCustomer(existingCustomer.Value);
        // Duplicated email
        var newCustomerWithDuplicatedEmail = CustomerFactory.JaneWithDuplicateEmail();
        // Duplicated personal info (Firstname, Lastname and DateOfBirth)
        var newCustomerWithDuplicatedPersonalInfo = CustomerFactory.JaneWithDuplicatePersonalInfo();

        // Act
        var duplicatedEmailResult = await _repository.CreateCustomer(newCustomerWithDuplicatedEmail.Value);
        var duplicatedEmailError = duplicatedEmailResult.Error.Message as Error;
        var duplicatedPersonalInfoResult = await _repository.CreateCustomer(newCustomerWithDuplicatedPersonalInfo.Value);
        var duplicatedPersonalInfoError = duplicatedPersonalInfoResult.Error.Message as Error;

        // Assert
        duplicatedEmailResult.IsFailure.Should().BeTrue();
        duplicatedEmailError?.Code.Should().Be((int)ErrorCodes.DuplicateCustomerByEmail);

        duplicatedPersonalInfoResult.IsFailure.Should().BeTrue();
        duplicatedPersonalInfoError?.Code.Should().Be((int)ErrorCodes.DuplicateCustomerByPersonalInfo);
    }

    [Fact]
    public async Task UpdateCustomer_ShouldUpdateCustomerSuccessfully_WhenUniqueCustomerIsGiven()
    {
        // Arrange
        var existingCustomer = CustomerFactory.John();
        await _repository.CreateCustomer(existingCustomer.Value);

        var modifiedCustomer = CustomerFactory.JohnUpdated().Value;

        existingCustomer.Value.Update(
            modifiedCustomer.FirstName.Value,
            modifiedCustomer.LastName.Value,
            modifiedCustomer.DateOfBirth.Value,
            modifiedCustomer.Phone.Value,
            modifiedCustomer.Email.Value,
            modifiedCustomer.BankAccount.Number);

        // Act
        var result = await _repository.UpdateCustomer(existingCustomer.Value);

        // Assert
        result.IsSuccess.Should().BeTrue();
        var updatedCustomer = await _dbContext.Set<Customer>().FirstOrDefaultAsync(c => c.Id == existingCustomer.Value.Id);
        updatedCustomer.Should().NotBeNull();
        updatedCustomer?.LastName.Value.Should().Be(modifiedCustomer.LastName.Value);
    }


    [Fact]
    public async Task UpdateCustomer_ShouldFailToCreateCustomer_WhenDuplicatedEmailOrPersonalInfoIsGiven()
    {
        var existingCustomer = CustomerFactory.Bill();
        await _repository.CreateCustomer(existingCustomer.Value);

        // Duplicated email
        var newCustomerWithDuplicatedEmail = CustomerFactory.BillWithDuplicateEmail();
        // Duplicated personal info (Firstname, Lastname and DateOfBirth)
        var newCustomerWithDuplicatedPersonalInfo = CustomerFactory.BillWithDuplicatePersonalInfo();

        // Act
        var duplicatedEmailResult = await _repository.UpdateCustomer(newCustomerWithDuplicatedEmail.Value);
        var duplicatedEmailError = duplicatedEmailResult.Error.Message as Error;

        var duplicatedPersonalInfoResult = await _repository.UpdateCustomer(newCustomerWithDuplicatedPersonalInfo.Value);
        var duplicatedPersonalInfoError = duplicatedPersonalInfoResult.Error.Message as Error;

        // Assert
        duplicatedEmailResult.IsFailure.Should().BeTrue();
        duplicatedEmailError?.Code.Should().Be((int)ErrorCodes.DuplicateCustomerByEmail);

        duplicatedPersonalInfoResult.IsFailure.Should().BeTrue();
        duplicatedPersonalInfoError?.Code.Should().Be((int)ErrorCodes.DuplicateCustomerByPersonalInfo);
    }

    private static class CustomerFactory
    {
        public static Result<Customer> Raymond() =>
            Customer.Create("Raymond", "Robin", new DateTime(1996, 6, 25), "+989145796432", "Raymond.Robin@gmail.com", "IR000000000000000000000011");

        public static Result<Customer> Khalil() =>
            Customer.Create("Khalil", "Khani", new DateTime(1989, 4, 21), "+989145799298", "khalil.khani2020@gmail.com", "IR000000000000000000000001");

        public static Result<Customer> John() =>
            Customer.Create("John", "Doe", new DateTime(1990, 1, 1), "+989145799298", "john.doe@test.com", "IR000000000000000000000001");

        public static Result<Customer> JohnUpdated() =>
            Customer.Create("John", "Wood", new DateTime(1990, 1, 1), "+989145799298", "john.doe@test.com", "IR000000000000000000000001");

        public static Result<Customer> Jane() =>
            Customer.Create("Jane", "Smith", new DateTime(1995, 6, 26), "+989353223636", "Jane@gmail.com", "IR000000000000000000000004");

        public static Result<Customer> JaneWithDuplicateEmail() =>
            Customer.Create("Jane", "Geller", new DateTime(1997, 8, 2), "+989353223637", "Jane@gmail.com", "IR000000000000000000000005");

        public static Result<Customer> JaneWithDuplicatePersonalInfo() =>
            Customer.Create("Jane", "Smith", new DateTime(1995, 6, 26), "+989353223638", "Jane.smith@gmail.com", "IR000000000000000000000006");


        public static Result<Customer> Bill() =>
            Customer.Create("Bill", "Gates", new DateTime(1965, 3, 25), "+989353222323", "Gates@gmail.com", "IR000000000000000000000007");

        public static Result<Customer> BillWithDuplicateEmail() =>
            Customer.Create("Bill", "Geller", new DateTime(1997, 8, 26), "+989353222324", "Gates@gmail.com", "IR000000000000000000000008");

        public static Result<Customer> BillWithDuplicatePersonalInfo() =>
            Customer.Create("Bill", "Gates", new DateTime(1965, 3, 25), "+989353222323", "Bill.Gates@gmail.com", "IR000000000000000000000009");
    }
}