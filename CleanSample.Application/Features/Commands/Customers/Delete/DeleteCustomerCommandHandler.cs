using CleanSample.Application.Abstractions.Messaging;
using CleanSample.Domain.Abstractions.Event;
using CleanSample.Domain.Abstractions.Repository;
using CleanSample.Utility;

namespace CleanSample.Application.Features.Commands.Customers.Delete;

public class DeleteCustomerCommandHandler :BaseHandler, ICommandHandler<DeleteCustomerCommand, bool>
{
    public DeleteCustomerCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IDomainEventDispatcher domainEventDispatcher)
        : base(unitOfWork, mapper, domainEventDispatcher)
    {
    }

    public async Task<Result<bool>> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await UnitOfWork.Customer.GetByIdAsync(request.Id);
        if (customer == null)
            return Result<bool>.Failure(new Error((int)HttpStatusCode.NotFound, "Customer not found!"));

        UnitOfWork.Customer.Remove(customer);
        await UnitOfWork.CompleteAsync();
        await DomainEventDispatcher.DispatchAsync(customer);

        return true;
    }
}