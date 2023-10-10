namespace CleanSample.Domain.Events;

public class CustomerDeletedDomainEvent : DomainEvent<int>
{
    public CustomerDeletedDomainEvent(int customerId) : base(customerId)
    {
    }
}