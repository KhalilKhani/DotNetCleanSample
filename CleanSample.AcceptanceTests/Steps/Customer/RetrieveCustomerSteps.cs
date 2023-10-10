using CleanSample.Application.DTOs;
using CleanSample.Application.Features.Commands.Customers.Create;
using CleanSample.Utility;

namespace CleanSample.AcceptanceTests.Steps.Customer
{
    [Binding]
    public class RetrieveCustomerSteps : BaseTest
    {
        private enum ContextKeys
        {
            CustomerDetails,
            RetrieveCustomerResult,
        }
        private readonly ScenarioContext _scenarioContext;

        public RetrieveCustomerSteps(ScenarioContext scenarioContext, WebAppFactory factory) : base(factory)
        {
            _scenarioContext = scenarioContext;
        }


        [Given(@"an existing customer with the following details:")]
        public async Task GivenAnExistingCustomerWithTheFollowingDetails(Table table)
        {
            var customerDetails = table.CreateInstance<CreateCustomerCommand>();
            var createResult = await ApiClientDriver.CreateCustomerAsync(customerDetails);
            createResult.IsSuccess.Should().BeTrue();
            _scenarioContext.Set(customerDetails, ContextKeys.CustomerDetails.ToString());

        }

        [When(@"a request is made to retrieve customer with Id (.*)")]
        public async Task WhenARequestIsMadeToRetrieveCustomerWithId(int id)
        {
            var result = await ApiClientDriver.GetCustomerByIdAsync(id);
            _scenarioContext.Set(result, ContextKeys.RetrieveCustomerResult.ToString());
        }

        [Then(@"the response of retrieving the customer should be successful")]
        public void ThenTheResponseOfRetrievingTheCustomerShouldBeSuccessful()
        {
            var result = _scenarioContext.Get<Result<CustomerDto>>(ContextKeys.RetrieveCustomerResult.ToString());
            result.IsSuccess.Should().BeTrue();
        }

        [Then(@"the retrieved customer details should be returned:")]
        public void ThenTheRetrievedCustomerDetailsShouldBeReturned(Table table)
        {
            var result = _scenarioContext.Get<Result<CustomerDto>>(ContextKeys.RetrieveCustomerResult.ToString());
            table.CompareToInstance(result.Value);
        }
    }
}
