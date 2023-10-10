namespace CleanSample.Domain.Abstractions.Event;

public interface IDomainEvent<out TId> : INotification
{
    Guid EventId { get; }
    DateTime OccurredOn { get; }
    TId AggregateId { get; }
}