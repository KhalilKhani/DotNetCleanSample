namespace CleanSample.UnitTests.Application.Features.Queries.Customers.Get;

public class GetCustomerQueryHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IDomainEventDispatcher> _domainEventDispatcherMock = new();
    private readonly GetCustomerQueryByIdHandler _queryByIdHandler;

    private readonly GetCustomerByIdQuery _validByIdQuery = new() { Id = 1 };
    private readonly Customer _existingCustomer;
    private readonly CustomerDto _existingCustomerDto;

    private readonly string _firstname = "Ray";
    private readonly string _lastname = "Mysterio";
    private readonly DateTime _dateOfBirth = new(1980, 01, 01);
    private readonly string _phoneNumber = "+989145799298";
    private readonly string _email = "Ray@gmail.com";
    private readonly string _bankAccountNumber = "IR000000000000000000000001";

    public GetCustomerQueryHandlerTests()
    {
        _existingCustomer = CreateExistingCustomer();
        _existingCustomerDto = CreateExistingCustomerDto();

        _unitOfWorkMock.Setup(u => u.Customer.GetByIdAsync(_validByIdQuery.Id)).ReturnsAsync(_existingCustomer);
        _mapperMock.Setup(m => m.Map<CustomerDto>(_existingCustomer)).Returns(_existingCustomerDto);

        _queryByIdHandler = new GetCustomerQueryByIdHandler(_unitOfWorkMock.Object, _mapperMock.Object, _domainEventDispatcherMock.Object);
    }

    private Customer CreateExistingCustomer()
    {
        return Customer.Create(_firstname, _lastname, _dateOfBirth, _phoneNumber, _email, _bankAccountNumber).Value;
    }

    private CustomerDto CreateExistingCustomerDto()
    {
        return new CustomerDto
        {
            Firstname = _firstname,
            Lastname = _lastname,
            DateOfBirth = _dateOfBirth,
            PhoneNumber = _phoneNumber,
            Email = _email,
            BankAccountNumber = _bankAccountNumber
        };
    }

    [Fact]
    public async Task Handle_ValidQuery_ShouldReturnSuccessResultWithCustomerDTO()
    {
        // Act
        var result = await _queryByIdHandler.Handle(_validByIdQuery, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(_existingCustomerDto);
        _unitOfWorkMock.Verify(u => u.Customer.GetByIdAsync(_validByIdQuery.Id), Times.Once);
        _mapperMock.Verify(m => m.Map<CustomerDto>(_existingCustomer), Times.Once);
    }
}