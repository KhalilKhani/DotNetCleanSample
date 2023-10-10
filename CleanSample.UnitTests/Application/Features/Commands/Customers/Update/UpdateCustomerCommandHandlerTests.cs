namespace CleanSample.UnitTests.Application.Features.Commands.Customers.Update;

public class UpdateCustomerCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IDomainEventDispatcher> _domainEventDispatcherMock = new();
    private readonly UpdateCustomerCommandHandler _commandHandler;

    private readonly UpdateCustomerCommand _validCommand = new()
    {
        Id = 1,
        Firstname = "Khalil",
        Lastname = "Khani",
        DateOfBirth = new DateTime(1980, 01, 01),
        PhoneNumber = "+989145799298",
        Email = "khalil.khani2020@gmail.com",
        BankAccountNumber = "IR000000000000000000000001"
    };
    private readonly CustomerDto _validCustomerDto;
    private readonly Customer _existingCustomer;

    public UpdateCustomerCommandHandlerTests()
    {
        _existingCustomer = CreateExistingCustomer();
        var validUpdatedCustomer = CreateValidUpdatedCustomer();
        _validCustomerDto = CreateValidCustomerDto();

        _unitOfWorkMock.Setup(u => u.Customer.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(_existingCustomer);
        _unitOfWorkMock.Setup(u => u.Customer.UpdateCustomer(It.IsAny<Customer>())).ReturnsAsync(Result<Customer>.Success(validUpdatedCustomer));
        _mapperMock.Setup(m => m.Map<CustomerDto>(It.IsAny<Customer>())).Returns(_validCustomerDto);

        _commandHandler = new UpdateCustomerCommandHandler(_unitOfWorkMock.Object, _mapperMock.Object, _domainEventDispatcherMock.Object);
    }

    private Customer CreateExistingCustomer()
    {
        return Customer.Create("ExistingFirstName", "ExistingLastName", new DateTime(1975, 01, 01), "+989145799298", "existing.customer@gmail.com", "IR000000000000000000000001").Value;
    }

    private Customer CreateValidUpdatedCustomer()
    {
        var customer = _existingCustomer.Update(_validCommand.Firstname, _validCommand.Lastname, _validCommand.DateOfBirth, _validCommand.PhoneNumber, _validCommand.Email, _validCommand.BankAccountNumber).Value;
        return customer;
    }

    private CustomerDto CreateValidCustomerDto()
    {
        return new CustomerDto
        {
            Firstname = _validCommand.Firstname,
            Lastname = _validCommand.Lastname,
            DateOfBirth = _validCommand.DateOfBirth,
            PhoneNumber = _validCommand.PhoneNumber,
            Email = _validCommand.Email,
            BankAccountNumber = _validCommand.BankAccountNumber
        };
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldReturnSuccessResultWithCustomerDTO()
    {
        // Act
        var result = await _commandHandler.Handle(_validCommand, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(_validCustomerDto);
        _unitOfWorkMock.Verify(u => u.Customer.UpdateCustomer(It.IsAny<Customer>()), Times.Once);
        _mapperMock.Verify(m => m.Map<CustomerDto>(It.IsAny<Customer>()), Times.Once);
        _domainEventDispatcherMock.Verify(d => d.DispatchAsync(It.IsAny<Customer>()), Times.Once);
    }
}