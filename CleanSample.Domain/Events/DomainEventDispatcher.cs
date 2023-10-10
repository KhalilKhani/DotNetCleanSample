using CleanSample.Domain.Abstractions.Entity;
using CleanSample.Domain.Abstractions.Event;

namespace CleanSample.Domain.Events;

public class DomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IPublisher _mediator;

    public DomainEventDispatcher(IPublisher mediator)
    {
        _mediator = mediator;
    }

    public async Task DispatchAsync<TId>(AggregateRoot<TId> aggregate)
    {
        foreach (var domainEvent in aggregate.DomainEvents) 
            await _mediator.Publish(domainEvent);

        aggregate.ClearDomainEvents();
    }
}