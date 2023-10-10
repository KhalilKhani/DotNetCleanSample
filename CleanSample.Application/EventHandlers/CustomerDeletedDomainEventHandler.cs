using CleanSample.Domain.Events;

namespace CleanSample.Application.EventHandlers;

public sealed class CustomerDeletedDomainEventHandler : INotificationHandler<CustomerDeletedDomainEvent>
{
    public Task Handle(CustomerDeletedDomainEvent notification, CancellationToken cancellationToken)
    {
        // Event handling logic
        Console.WriteLine("Customer deleted with id {0}", notification.AggregateId);
        return Task.CompletedTask;
    }
}