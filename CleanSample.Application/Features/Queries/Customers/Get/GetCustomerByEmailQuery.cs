using CleanSample.Application.Abstractions.Messaging;
using CleanSample.Application.DTOs;

namespace CleanSample.Application.Features.Queries.Customers.Get;

public class GetCustomerByEmailQuery : IQuery<CustomerDto>
{
    public string Email { get; set; }
}