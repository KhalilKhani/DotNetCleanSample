using CleanSample.Domain.Aggregates;
using CleanSample.Utility;

namespace CleanSample.Domain.Abstractions.Repository;

public interface ICustomerRepository : IBaseRepository<Customer>
{
    Task<Result<Customer>> GetByEmail(string email);
    Task<Result<Customer>> CreateCustomer(Customer customer);
    Task<Result<Customer>> UpdateCustomer(Customer customer);
}
