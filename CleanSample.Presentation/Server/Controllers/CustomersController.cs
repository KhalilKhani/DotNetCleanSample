using CleanSample.Application.DTOs;
using CleanSample.Application.Features.Commands.Customers.Create;
using CleanSample.Application.Features.Commands.Customers.Delete;
using CleanSample.Application.Features.Commands.Customers.Update;
using CleanSample.Application.Features.Queries.Customers.Get;
using CleanSample.Application.Features.Queries.Customers.GetAll;

namespace CleanSample.Presentation.Server.Controllers;

public class CustomersController : BaseApiController
{
    public CustomersController(ILogger<CustomersController> logger, IMediator mediator) : base(logger, mediator)
    {
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomerDto>>> GetAllCustomers()
    {
        GetAllCustomersQuery query = new();
        var result = await Mediator.Send(query);

        return ExtractResult(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CustomerDto>> GetCustomerById(int id)
    {
        GetCustomerByIdQuery byIdQuery = new() { Id = id };
        var result = await Mediator.Send(byIdQuery);

        return ExtractResult(result);
    }

    [HttpGet("{email}")]
    public async Task<ActionResult<CustomerDto>> GetCustomerByEmail(string email)
    {
        GetCustomerByEmailQuery byEmailQuery = new() { Email= email };
        var result = await Mediator.Send(byEmailQuery);

        return ExtractResult(result);
    }

    [HttpPost]
    public async Task<ActionResult<CustomerDto>> CreateCustomer([FromBody] CreateCustomerCommand command)
    {
        var result = await Mediator.Send(command);

        return ExtractResult(result);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<CustomerDto>> UpdateCustomer(int id, [FromBody] UpdateCustomerCommand command)
    {
        if (id != command.Id)
            return BadRequest("Invalid ID");

        var result = await Mediator.Send(command);

        return ExtractResult(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<bool>> DeleteCustomer(int id)
    {
        DeleteCustomerCommand command = new() { Id = id };
        var result = await Mediator.Send(command);

        return ExtractResult(result);
    }
}
