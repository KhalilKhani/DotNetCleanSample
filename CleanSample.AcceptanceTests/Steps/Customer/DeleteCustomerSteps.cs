using CleanSample.Application.DTOs;
using CleanSample.Application.Features.Commands.Customers.Create;
using CleanSample.Utility;

namespace CleanSample.AcceptanceTests.Steps.Customer
{
    [Binding]
    public class DeleteCustomerSteps : BaseTest
    {
        private enum ContextKeys
        {
            RetrieveCustomerResult,
            DeleteCustomerResult,
        }
        private readonly ScenarioContext _scenarioContext;

        public DeleteCustomerSteps(ScenarioContext scenarioContext, WebAppFactory factory) : base(factory)
        {
            _scenarioContext = scenarioContext;
        }

        [BeforeScenario("@delete", Order = 1)]
        public async Task CreateInitialUser()
        {
            CreateCustomerCommand createCustomerCommand = new()
            {
                Firstname = "Khalil",
                Lastname = "Khani",
                DateOfBirth = new DateTime(1990, 01, 01),
                PhoneNumber = "+989145799298",
                Email = "Khalil.Khani@example.com",
                BankAccountNumber = "IR000000000000001"
            };
            var createResult = await ApiClientDriver.CreateCustomerAsync(createCustomerCommand);
            createResult.IsSuccess.Should().BeTrue();
        }

        [Given(@"an existing customer \(to be deleted\) with the Id (.*)")]
        public async Task GivenAnExistingCustomerToBeDeletedWithTheId(int id)
        {
            var result = await ApiClientDriver.GetCustomerByIdAsync(id);
            result.IsSuccess.Should().BeTrue();
            _scenarioContext.Set(result, ContextKeys.RetrieveCustomerResult.ToString());
        }

        [When(@"a request is made to delete customer with Id (.*)")]
        public async Task WhenARequestIsMadeToDeleteCustomerWithId(int id)
        {
            var result = await ApiClientDriver.DeleteCustomerAsync(id);
            _scenarioContext.Set(result, ContextKeys.DeleteCustomerResult.ToString());
        }

        [Then(@"the response of deleting the customer should be successful")]
        public void ThenTheResponseOfDeletingTheCustomerShouldBeSuccessful()
        {
            var result = _scenarioContext.Get<Result>(ContextKeys.DeleteCustomerResult.ToString());
            result.IsSuccess.Should().BeTrue();
        }

        [When(@"a request is made to retrieve deleted customer with Id (.*)")]
        public async Task WhenARequestIsMadeToRetrieveDeletedCustomerWithId(int id)
        {
            var result = await ApiClientDriver.GetCustomerByIdAsync(id);
            _scenarioContext.Set(result, ContextKeys.RetrieveCustomerResult.ToString());
        }

        [Then(@"the response of delete should be not found")]
        public void ThenTheResponseShouldBeNotFound()
        {
            var result = _scenarioContext.Get<Result<CustomerDto>>(ContextKeys.RetrieveCustomerResult.ToString());
            result.IsSuccess.Should().BeFalse();
            result.Error.Code.Should().Be(404);
        }
    }
}
