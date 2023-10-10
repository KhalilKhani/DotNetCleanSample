using CleanSample.Application.Abstractions.Messaging;
using CleanSample.Application.DTOs;
using CleanSample.Domain.Abstractions.Event;
using CleanSample.Domain.Abstractions.Repository;
using CleanSample.Domain.Aggregates;
using CleanSample.Utility;

namespace CleanSample.Application.Features.Commands.Customers.Create;

public class CreateCustomerCommandHandler : BaseHandler, ICommandHandler<CreateCustomerCommand, CustomerDto>
{

    public CreateCustomerCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IDomainEventDispatcher domainEventDispatcher)
        : base(unitOfWork, mapper, domainEventDispatcher)
    {
    }

    public async Task<Result<CustomerDto>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customerResult = Customer.Create(
            request.Firstname,
            request.Lastname,
            request.DateOfBirth,
            request.PhoneNumber,
            request.Email,
            request.BankAccountNumber);

        if (customerResult.IsFailure)
            return Result<CustomerDto>.Failure(customerResult.Error);

        var createResult = await UnitOfWork.Customer.CreateCustomer(customerResult.Value);
        if (createResult.IsFailure)
            return Result<CustomerDto>.Failure(createResult.Error);

        await DomainEventDispatcher.DispatchAsync(customerResult.Value);

        return Mapper.Map<CustomerDto>(customerResult.Value)!;
    }
}