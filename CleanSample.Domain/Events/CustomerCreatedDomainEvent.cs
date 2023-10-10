using CleanSample.Domain.Aggregates;

namespace CleanSample.Domain.Events;

public class CustomerCreatedDomainEvent : DomainEvent<int>
{
    public Customer Customer { get; }

    public CustomerCreatedDomainEvent(Customer customer) : base(customer.Id)
    {
        Customer = customer;
    }
}