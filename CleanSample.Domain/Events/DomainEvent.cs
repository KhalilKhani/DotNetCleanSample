using CleanSample.Domain.Abstractions.Event;

namespace CleanSample.Domain.Events;

public class DomainEvent<TId> : IDomainEvent<TId>
{
    public DomainEvent(TId aggregateId)
    {
        AggregateId = aggregateId;
    }

    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public TId AggregateId { get; }
}