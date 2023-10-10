using CleanSample.Application.Abstractions.Messaging;
using CleanSample.Application.DTOs;
using CleanSample.Domain.Abstractions.Event;
using CleanSample.Domain.Abstractions.Repository;
using CleanSample.Utility;

namespace CleanSample.Application.Features.Queries.Customers.Get;

public class GetCustomerQueryByEmailHandler : BaseHandler, IQueryHandler<GetCustomerByEmailQuery, CustomerDto>
{
    public GetCustomerQueryByEmailHandler(IUnitOfWork unitOfWork, IMapper mapper, IDomainEventDispatcher domainEventDispatcher)
        : base(unitOfWork, mapper, domainEventDispatcher)
    {
    }

    public async Task<Result<CustomerDto>> Handle(GetCustomerByEmailQuery request, CancellationToken cancellationToken)
    {
        var customer = await UnitOfWork.Customer.GetByEmail(request.Email);
        if (customer.IsFailure)
            return customer.Error;

        return Mapper.Map<CustomerDto>(customer.Value)!;
    }
}