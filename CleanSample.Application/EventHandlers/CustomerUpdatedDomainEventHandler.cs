using CleanSample.Domain.Events;

namespace CleanSample.Application.EventHandlers;

public sealed class CustomerUpdatedDomainEventHandler : INotificationHandler<CustomerUpdatedDomainEvent>
{
    public Task Handle(CustomerUpdatedDomainEvent notification, CancellationToken cancellationToken)
    {
        // Event handling logic
        Console.WriteLine("Customer Updated: {0} {1}", notification.Customer.FirstName, notification.Customer.LastName);
        return Task.CompletedTask;
    }
}