using CleanSample.Application.Abstractions.Messaging;

namespace CleanSample.Application.Features.Commands.Customers.Delete;

public class DeleteCustomerCommand : ICommand<bool>
{
    public int Id { get; set; }
}