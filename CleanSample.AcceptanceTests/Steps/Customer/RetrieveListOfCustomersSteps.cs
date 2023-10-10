using CleanSample.Application.DTOs;
using CleanSample.Application.Features.Commands.Customers.Create;
using CleanSample.Utility;

namespace CleanSample.AcceptanceTests.Steps.Customer
{
    [Binding]
    public class RetrieveListOfCustomersSteps : BaseTest
    {
        private enum ContextKeys
        {
            CustomerList
        }

        private readonly ScenarioContext _scenarioContext;

        public RetrieveListOfCustomersSteps(ScenarioContext scenarioContext, WebAppFactory factory) : base(factory)
        {
            _scenarioContext = scenarioContext;
        }

        [Given(@"the following customers exist:")]
        public async Task GivenTheFollowingCustomersExist(Table table)
        {
            foreach (var row in table.Rows)
            {
                var customer = new CreateCustomerCommand
                {
                    Firstname = row["Firstname"],
                    Lastname = row["Lastname"],
                    DateOfBirth = DateTime.Parse(row["DateOfBirth"]),
                    PhoneNumber = row["PhoneNumber"],
                    Email = row["Email"],
                    BankAccountNumber = row["BankAccountNumber"]
                };

                var result = await ApiClientDriver.CreateCustomerAsync(customer);
                result.IsSuccess.Should().BeTrue();
            }
        }

        [When(@"a request is made to list all customers")]
        public async Task WhenARequestIsMadeToListAllCustomers()
        {
            var result = await ApiClientDriver.GetAllCustomersAsync();
            _scenarioContext.Set(result, ContextKeys.CustomerList.ToString());
        }

        [Then(@"the response of getting all customers should be successful")]
        public void ThenTheResponseOfGettingAllCustomersShouldBeSuccessful()
        {
            var listResult = _scenarioContext.Get<Result<IEnumerable<CustomerDto>>>(ContextKeys.CustomerList.ToString());
            listResult.IsSuccess.Should().BeTrue();
        }

        [Then(@"the response should contain the following customer details:")]
        public void ThenTheResponseShouldContainTheFollowingCustomerDetails(Table table)
        {
            var expectedCustomers = table.CreateSet<CustomerDto>().ToList();
            var listResult = _scenarioContext.Get<Result<IEnumerable<CustomerDto>>>(ContextKeys.CustomerList.ToString());
            listResult.Value.Should().BeEquivalentTo(expectedCustomers);
        }
    }
}
