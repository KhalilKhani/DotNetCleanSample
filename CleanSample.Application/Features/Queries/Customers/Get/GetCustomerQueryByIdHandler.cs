using CleanSample.Application.Abstractions.Messaging;
using CleanSample.Application.DTOs;
using CleanSample.Domain.Abstractions.Event;
using CleanSample.Domain.Abstractions.Repository;
using CleanSample.Utility;

namespace CleanSample.Application.Features.Queries.Customers.Get;

public class GetCustomerQueryByIdHandler : BaseHandler, IQueryHandler<GetCustomerByIdQuery, CustomerDto>
{
    public GetCustomerQueryByIdHandler(IUnitOfWork unitOfWork, IMapper mapper, IDomainEventDispatcher domainEventDispatcher)
        : base(unitOfWork, mapper, domainEventDispatcher)
    {
    }

    public async Task<Result<CustomerDto>> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        var customer = await UnitOfWork.Customer.GetByIdAsync(request.Id);
        if (customer == null)
            return Result<CustomerDto>.Failure(new Error((int)HttpStatusCode.NotFound, "Customer Not found"));

        return Mapper.Map<CustomerDto>(customer)!;
    }
}