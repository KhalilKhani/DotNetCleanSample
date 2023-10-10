using CleanSample.Domain.Events;

namespace CleanSample.Application.EventHandlers;

public sealed class CustomerCreatedDomainEventHandler : INotificationHandler<CustomerCreatedDomainEvent>
{
    public Task Handle(CustomerCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        // Event handling logic
        Console.WriteLine("Customer Created: {0} {1}", notification.Customer.FirstName, notification.Customer.LastName);
        return Task.CompletedTask;
    }
}