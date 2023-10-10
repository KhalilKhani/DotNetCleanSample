using CleanSample.Application.Abstractions.Messaging;
using CleanSample.Application.DTOs;

#pragma warning disable CS8618
namespace CleanSample.Application.Features.Commands.Customers.Update;

public class UpdateCustomerCommand : ICommand<CustomerDto>
{
    public int Id { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string BankAccountNumber { get; set; }
}