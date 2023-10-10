using CleanSample.Domain.Abstractions.Entity;

namespace CleanSample.Domain.Abstractions.Event;

public interface IDomainEventDispatcher
{
    Task DispatchAsync<TId>(AggregateRoot<TId> aggregate);
}