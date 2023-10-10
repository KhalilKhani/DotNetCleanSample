using CleanSample.Domain.Abstractions.Event;
using CleanSample.Domain.Abstractions.Repository;

namespace CleanSample.Application.Abstractions.Messaging;

public abstract class BaseHandler
{
    protected readonly IMapper Mapper;
    protected readonly IUnitOfWork UnitOfWork;
    protected readonly IDomainEventDispatcher DomainEventDispatcher;

    protected BaseHandler(IUnitOfWork unitOfWork, IMapper mapper,
        IDomainEventDispatcher domainEventDispatcher)
    {
        UnitOfWork = unitOfWork;
        Mapper = mapper;
        DomainEventDispatcher = domainEventDispatcher;
    }
}