namespace CleanSample.UnitTests.Application.Features.Queries.Customers.GetAll;

public class GetAllCustomersQueryHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IDomainEventDispatcher> _domainEventDispatcherMock = new();
    private readonly GetAllCustomersQueryHandler _queryHandler;

    private readonly GetAllCustomersQuery _validQuery;
    private readonly List<CustomerDto> _existingCustomersDtos;

    private readonly List<(string firstName, string lastName, DateTime dateOfBirth, string phoneNumber, string email, string bankAccountNumber)> _customerData = new()
    {
        ("John", "Cena", new DateTime(1980, 01, 01), "+989145799298", "John@gmail.com", "IR000000000000000000000001"),
        ("Brock", "Lesnar", new DateTime(1990, 02, 02), "+989145799299", "Brock@gmail.com", "IR000000000000000000000002")
    };

    public GetAllCustomersQueryHandlerTests()
    {
        _validQuery = new GetAllCustomersQuery();

        List<Customer> existingCustomers = CreateExistingCustomers();
        _existingCustomersDtos = CreateExistingCustomersDtos();

        _unitOfWorkMock.Setup(u => u.Customer.GetAllAsync()).ReturnsAsync(existingCustomers);
        _mapperMock.Setup(m => m.Map<IEnumerable<CustomerDto>>(It.IsAny<List<Customer>>())).Returns(_existingCustomersDtos);

        _queryHandler = new GetAllCustomersQueryHandler(_unitOfWorkMock.Object, _mapperMock.Object, _domainEventDispatcherMock.Object);
    }

    private List<Customer> CreateExistingCustomers()
    {
        var customers = new List<Customer>();

        foreach (var (firstName, lastName, dateOfBirth, phoneNumber, email, bankAccountNumber) in _customerData)
        {
            customers.Add(Customer.Create(firstName, lastName, dateOfBirth, phoneNumber, email, bankAccountNumber).Value);
        }

        return customers;
    }

    private List<CustomerDto> CreateExistingCustomersDtos()
    {
        List<CustomerDto> customerDtos = new();

        foreach ((var firstName, var lastName, var dateOfBirth, var phoneNumber, var email, var bankAccountNumber) in _customerData)
        {
            customerDtos.Add(new CustomerDto
            {
                Firstname = firstName,
                Lastname = lastName,
                DateOfBirth = dateOfBirth,
                PhoneNumber = phoneNumber,
                Email = email,
                BankAccountNumber = bankAccountNumber
            });
        }

        return customerDtos;
    }

    [Fact]
    public async Task Handle_ValidQuery_ShouldReturnSuccessResultWithAllCustomersDTOs()
    {
        // Act
        var result = await _queryHandler.Handle(_validQuery, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(_existingCustomersDtos);
        _unitOfWorkMock.Verify(u => u.Customer.GetAllAsync(), Times.Once);
        _mapperMock.Verify(m => m.Map<IEnumerable<CustomerDto>>(It.IsAny<List<Customer>>()), Times.Once);
    }
}