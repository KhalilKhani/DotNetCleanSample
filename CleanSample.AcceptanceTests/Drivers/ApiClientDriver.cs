using CleanSample.Application.DTOs;
using CleanSample.Application.Features.Commands.Customers.Create;
using CleanSample.Application.Features.Commands.Customers.Update;
using CleanSample.Utility;

namespace CleanSample.AcceptanceTests.Drivers;

[Binding]
public class ApiClientDriver
{
    private readonly HttpApiClient _client;

    public ApiClientDriver()
    {
        _client = new HttpApiClient("https://localhost:7055/api/");
    }

    public ApiClientDriver(HttpClient httpClient)
    {
        _client = new HttpApiClient(httpClient);
    }

    public async Task<Result<CustomerDto>> CreateCustomerAsync(CreateCustomerCommand customer)
    {
        return await _client.SendRequestAsync<CustomerDto>(HttpMethod.Post, "/api/customers", customer);
    }

    public async Task<Result<CustomerDto>> GetCustomerByIdAsync(int id)
    {
        return await _client.SendRequestAsync<CustomerDto>(HttpMethod.Get, $"/api/customers/{id}");
    }
    public async Task<Result<CustomerDto>> GetCustomerByEmailAsync(string email)
    {
        return await _client.SendRequestAsync<CustomerDto>(HttpMethod.Get, $"/api/customers/{email}");
    }

    public async Task<Result<IEnumerable<CustomerDto>>> GetAllCustomersAsync()
    {
        return await _client.SendRequestAsync<IEnumerable<CustomerDto>>(HttpMethod.Get, "/api/customers");
    }

    public async Task<Result<CustomerDto>> UpdateCustomerAsync(int id, UpdateCustomerCommand customer)
    {
        return await _client.SendRequestAsync<CustomerDto>(HttpMethod.Put, $"/api/customers/{id}", customer);
    }

    public async Task<Result> DeleteCustomerAsync(int id)
    {
        return await _client.SendRequestAsync(HttpMethod.Delete, $"/api/customers/{id}");
    }
}