using CleanSample.Domain.Abstractions.Event;

namespace CleanSample.Domain.Abstractions.Entity;

public abstract class AggregateRoot<TId>: Entity<TId>
{
    private readonly List<IDomainEvent<TId>> _domainEvents = new();

    public IReadOnlyList<IDomainEvent<TId>> DomainEvents => _domainEvents;

    protected AggregateRoot()
    {
    }

    protected AggregateRoot(TId id) : base(id)
    {
    }

    protected void RaiseDomainEvent(IDomainEvent<TId> domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }


    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

}