using CleanSample.Application.Abstractions.Messaging;
using CleanSample.Application.DTOs;

namespace CleanSample.Application.Features.Queries.Customers.Get;

public class GetCustomerByIdQuery : IQuery<CustomerDto>
{
    public int Id { get; set; }
}