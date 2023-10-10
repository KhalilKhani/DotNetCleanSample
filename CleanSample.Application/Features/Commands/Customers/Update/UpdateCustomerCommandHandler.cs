using CleanSample.Application.Abstractions.Messaging;
using CleanSample.Application.DTOs;
using CleanSample.Domain.Abstractions.Event;
using CleanSample.Domain.Abstractions.Repository;
using CleanSample.Utility;

namespace CleanSample.Application.Features.Commands.Customers.Update;

public class UpdateCustomerCommandHandler : BaseHandler, ICommandHandler<UpdateCustomerCommand, CustomerDto>
{
    public UpdateCustomerCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IDomainEventDispatcher domainEventDispatcher)
        : base(unitOfWork, mapper, domainEventDispatcher)
    {
    }


    public async Task<Result<CustomerDto>> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {

        var customer = await UnitOfWork.Customer.GetByIdAsync(request.Id);
        if (customer == null)
            return Result<CustomerDto>.Failure(new Error((int)HttpStatusCode.NotFound, "Customer not found"));

        var customerResult = customer.Update(
            request.Firstname,
            request.Lastname,
            request.DateOfBirth,
            request.PhoneNumber,
            request.Email,
            request.BankAccountNumber);

        if (customerResult.IsFailure)
            return Result<CustomerDto>.Failure(customerResult.Error);

        var updateResult = await UnitOfWork.Customer.UpdateCustomer(customerResult.Value);
        if (updateResult.IsFailure)
            return Result<CustomerDto>.Failure(updateResult.Error);

        await DomainEventDispatcher.DispatchAsync(customerResult.Value);

        return Mapper.Map<CustomerDto>(customerResult.Value)!;
    }
}