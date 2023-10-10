using CleanSample.Application.Abstractions.Messaging;
using CleanSample.Application.DTOs;
using CleanSample.Domain.Abstractions.Event;
using CleanSample.Domain.Abstractions.Repository;
using CleanSample.Utility;

namespace CleanSample.Application.Features.Queries.Customers.GetAll;

public class GetAllCustomersQueryHandler :BaseHandler, IQueryHandler<GetAllCustomersQuery, IEnumerable<CustomerDto>>
{
    public GetAllCustomersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IDomainEventDispatcher domainEventDispatcher)
        : base(unitOfWork, mapper, domainEventDispatcher)
    {
    }

    public async Task<Result<IEnumerable<CustomerDto>>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
    {
        var customers = await UnitOfWork.Customer.GetAllAsync();
        var mappedList = Mapper.Map<IEnumerable<CustomerDto>>(customers);

        return Result<IEnumerable<CustomerDto>>.Success(mappedList);
    }
}