namespace CleanSample.UnitTests.Application.Features.Commands.Customers.Delete;

public class DeleteCustomerCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IDomainEventDispatcher> _domainEventDispatcherMock = new();
    private readonly DeleteCustomerCommandHandler _commandHandler;

    private readonly DeleteCustomerCommand _validCommand = new() { Id = 1 };

    public DeleteCustomerCommandHandlerTests()
    {
        var existingCustomer = CreateExistingCustomer();

        _unitOfWorkMock.Setup(u => u.Customer.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(existingCustomer);
        _unitOfWorkMock.Setup(u => u.Customer.Remove(existingCustomer));

        _commandHandler = new DeleteCustomerCommandHandler(_unitOfWorkMock.Object, _mapperMock.Object, _domainEventDispatcherMock.Object);
    }

    private Customer CreateExistingCustomer()
    {
        return Customer.Create("ExistingFirstName", "ExistingLastName", new DateTime(1975, 01, 01), "+989145799298", "existing.customer@gmail.com", "IR000000000000000000000001").Value;
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldReturnSuccessResultWithTrue()
    {
        // Act
        var result = await _commandHandler.Handle(_validCommand, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeTrue();
        _unitOfWorkMock.Verify(u => u.Customer.Remove(It.IsAny<Customer>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
        _domainEventDispatcherMock.Verify(d => d.DispatchAsync(It.IsAny<Customer>()), Times.Once);
    }
}