using CleanSample.Application.DTOs;
using CleanSample.Application.Features.Commands.Customers.Create;
using CleanSample.Utility;

namespace CleanSample.AcceptanceTests.Steps.Customer;

[Binding]
public class CreateCustomerSteps : BaseTest
{
    private enum ContextKeys
    {
        CreateCustomerCommand,
        CreateCustomerResult
    }

    private readonly ScenarioContext _scenarioContext;

    public CreateCustomerSteps(ScenarioContext scenarioContext, WebAppFactory factory) : base(factory)
    {
        _scenarioContext = scenarioContext;
    }


    [Given(@"the customer details:")]
    public void GivenTheCustomerDetails(Table table)
    {
        var createCustomerCommand = table.CreateInstance<CreateCustomerCommand>();
        _scenarioContext.Set(createCustomerCommand, ContextKeys.CreateCustomerCommand.ToString());
    }


    [When(@"a request is made to create a new customer")]
    public async Task WhenARequestIsMadeToCreateANewCustomer()
    {
        var createCustomerCommand = _scenarioContext.Get<CreateCustomerCommand>(ContextKeys.CreateCustomerCommand.ToString());
        var result = await ApiClientDriver.CreateCustomerAsync(createCustomerCommand);
        _scenarioContext.Set(result, ContextKeys.CreateCustomerResult.ToString());
    }


    [Then(@"the response of creating the customer should be successful")]
    public void ThenTheResponseOfCreatingTheCustomerShouldBeSuccessful()
    {
        var result = _scenarioContext.Get<Result<CustomerDto>>(ContextKeys.CreateCustomerResult.ToString());
        result.IsSuccess.Should().BeTrue();
    }


    [Then(@"the created customer details should be returned:")]
    public void ThenTheCreatedCustomerDetailsShouldBeReturned(Table table)
    {
        var result = _scenarioContext.Get<Result<CustomerDto>>(ContextKeys.CreateCustomerResult.ToString());
        table.CompareToInstance(result.Value);
    }


    [When(@"a request is made to create a new customer with the same details")]
    public async Task WhenARequestIsMadeToCreateANewCustomerWithTheSameDetails()
    {
        var createCustomerCommand = _scenarioContext.Get<CreateCustomerCommand>(ContextKeys.CreateCustomerCommand.ToString());
        var result = await ApiClientDriver.CreateCustomerAsync(createCustomerCommand);
        _scenarioContext.Set(result, ContextKeys.CreateCustomerResult.ToString());
    }


    [Then(@"the response should be BadRequest due to uniqueness violation")]
    public void ThenTheResponseShouldBeBadRequestDueToUniquenessViolation()
    {
        var result = _scenarioContext.Get<Result<CustomerDto>>(ContextKeys.CreateCustomerResult.ToString());
        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be(400);
    }
}
