using CleanSample.Domain.Aggregates;

namespace CleanSample.Domain.Events;

public class CustomerUpdatedDomainEvent : DomainEvent<int>
{
    public Customer Customer { get; }

    public CustomerUpdatedDomainEvent(Customer customer) : base(customer.Id)
    {
        Customer = customer;
    }
}