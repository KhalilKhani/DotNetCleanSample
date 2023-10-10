using CleanSample.Application.DTOs;
using CleanSample.Application.Features.Commands.Customers.Create;
using CleanSample.Application.Features.Commands.Customers.Update;
using CleanSample.Utility;

namespace CleanSample.AcceptanceTests.Steps.Customer
{
    [Binding]
    public class UpdateCustomerSteps : BaseTest
    {
        private enum ContextKeys
        {
            UpdateCustomerResult,
        }
        private readonly ScenarioContext _scenarioContext;

        public UpdateCustomerSteps(ScenarioContext scenarioContext, WebAppFactory factory) : base(factory)
        {
            _scenarioContext = scenarioContext;
        }


        [Given(@"existing customers with the following details:")]
        public async Task GivenExistingCustomersWithTheFollowingDetails(Table table)
        {
            var customers = table.CreateSet<CreateCustomerCommand>();
            foreach (var customer in customers)
            {
                var createResult = await ApiClientDriver.CreateCustomerAsync(customer);
                createResult.IsSuccess.Should().BeTrue();
            }
        }

        [When(@"a request is made to update customer with Id (.*) and the following updated details:")]
        public async Task WhenARequestIsMadeToUpdateCustomerWithIdAndTheFollowingUpdatedDetails(int id, Table table)
        {
            var updatedCustomer = table.CreateInstance<UpdateCustomerCommand>();
            var result = await ApiClientDriver.UpdateCustomerAsync(id, updatedCustomer);
            _scenarioContext.Set(result, ContextKeys.UpdateCustomerResult.ToString());
        }

        [Then(@"the response of updating the customer should be successful")]
        public void ThenTheResponseOfUpdatingTheCustomerShouldBeSuccessful()
        {
            var result = _scenarioContext.Get<Result<CustomerDto>>(ContextKeys.UpdateCustomerResult.ToString());
            result.IsSuccess.Should().BeTrue();
        }

        [Then(@"the updated customer details should be returned:")]
        public void ThenTheUpdatedCustomerDetailsShouldBeReturned(Table table)
        {
            var result = _scenarioContext.Get<Result<CustomerDto>>(ContextKeys.UpdateCustomerResult.ToString());
            table.CompareToInstance(result.Value);
        }

        [When(@"a request is made to update customer with Id (.*) and the following duplicated details:")]
        public async Task WhenARequestIsMadeToUpdateCustomerWithIdAndTheFollowingDuplicatedDetails(int id, Table table)
        {
            var updatedCustomer = table.CreateInstance<UpdateCustomerCommand>();
            var result = await ApiClientDriver.UpdateCustomerAsync(id, updatedCustomer);
            _scenarioContext.Set(result, ContextKeys.UpdateCustomerResult.ToString());
        }

        [Then(@"the response of update should be BadRequest due to uniqueness violation")]
        public void ThenTheResponseShouldBeBadRequestDueToUniquenessViolation()
        {
            var result = _scenarioContext.Get<Result<CustomerDto>>(ContextKeys.UpdateCustomerResult.ToString());
            result.IsSuccess.Should().BeFalse();
            result.Error.Code.Should().Be(400);
        }
    }
}
