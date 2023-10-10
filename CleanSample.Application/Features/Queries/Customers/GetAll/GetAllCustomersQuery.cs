using CleanSample.Application.Abstractions.Messaging;
using CleanSample.Application.DTOs;

namespace CleanSample.Application.Features.Queries.Customers.GetAll;

public class GetAllCustomersQuery : IQuery<IEnumerable<CustomerDto>>
{

}