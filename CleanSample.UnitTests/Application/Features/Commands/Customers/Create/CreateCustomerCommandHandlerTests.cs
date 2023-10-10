namespace CleanSample.UnitTests.Application.Features.Commands.Customers.Create;

public class CreateCustomerCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IDomainEventDispatcher> _domainEventDispatcherMock = new();
    private readonly CreateCustomerCommandHandler _commandHandler;
    private readonly CreateCustomerCommand _validCommand = new()
    {
        Firstname = "Khalil",
        Lastname = "Khani",
        DateOfBirth = new DateTime(1980, 01, 01),
        PhoneNumber = "+989145799298",
        Email = "khalil.khani2020@gmail.com",
        BankAccountNumber = "IR000000000000000000000001"
    };
    private readonly CustomerDto _validCustomerDto;


    public CreateCustomerCommandHandlerTests()
    {
        _commandHandler = new CreateCustomerCommandHandler(_unitOfWorkMock.Object, _mapperMock.Object, _domainEventDispatcherMock.Object);

        var validCreatedCustomer = CreateValidCustomer();
        _validCustomerDto = CreateValidCustomerDto();

        _unitOfWorkMock.Setup(u => u.Customer.CreateCustomer(It.IsAny<Customer>())).ReturnsAsync(Result<Customer>.Success(validCreatedCustomer));
        _mapperMock.Setup(m => m.Map<CustomerDto>(It.IsAny<Customer>())).Returns(_validCustomerDto);
    }


    private Customer CreateValidCustomer()
    {
        return Customer.Create(_validCommand.Firstname, _validCommand.Lastname, _validCommand.DateOfBirth, _validCommand.PhoneNumber, _validCommand.Email, _validCommand.BankAccountNumber).Value;
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
        _unitOfWorkMock.Verify(u => u.Customer.CreateCustomer(It.IsAny<Customer>()), Times.Once);
        _mapperMock.Verify(m => m.Map<CustomerDto>(It.IsAny<Customer>()), Times.Once);
        _domainEventDispatcherMock.Verify(d => d.DispatchAsync(It.IsAny<Customer>()), Times.Once);
    }
}
