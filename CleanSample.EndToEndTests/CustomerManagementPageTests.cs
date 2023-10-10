using FluentAssertions;

namespace CleanSample.EndToEndTests;

public class CustomerManagementPageTests : IDisposable
{
    private readonly IWebDriver _driver;
    private readonly CustomerPageModel _customerPageModel;

    public CustomerManagementPageTests()
    {
        _driver = new ChromeDriver(new ChromeOptions());
        _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        _customerPageModel = new CustomerPageModel(_driver);
    }

    [Fact]
    public void RunCustomerTestsInOrder()
    {
        AddCustomer_EndToEndTests();
        Thread.Sleep(3000);
        UpdateCustomer_EndToEndTests();
        Thread.Sleep(3000);
        DeleteCustomer_EndToEndTests();
        Thread.Sleep(1000);
    }

    private void AddCustomer_EndToEndTests()
    {
        _customerPageModel.GoTo();

        var customer = CustomerFactory.John();
        _customerPageModel.AddCustomer(customer);
        _customerPageModel.WaitForModalToClose();
        var isCustomerAdded = _customerPageModel.IsCustomerInTable(customer);

        isCustomerAdded.Should().BeTrue("The new customer data should be displayed in the table.");
    }

    private void UpdateCustomer_EndToEndTests()
    {
        var updatedCustomer = CustomerFactory.Jane();

        _customerPageModel.UpdateCustomer(updatedCustomer);
        _customerPageModel.WaitForModalToClose();
        var isCustomerUpdated = _customerPageModel.IsCustomerInTable(updatedCustomer);

        isCustomerUpdated.Should().BeTrue("The customer details should be updated in the table.");
    }


    private void DeleteCustomer_EndToEndTests()
    {
        var firstCustomerRow = _driver.FindElement(By.CssSelector("#CustomersTable tbody tr:first-child"));
        string customerEmail = firstCustomerRow.FindElement(By.CssSelector(".email")).Text;

        _customerPageModel.DeleteFirstCustomer();
        // ToDo: Check if deleted item is gone from the table in a safe way
        // var rows = _driver.FindElements(By.CssSelector("#CustomersTable tbody tr"));
        // bool customerStillExists = rows.Any(r => r.Text.Contains(customerEmail));
        // customerStillExists.Should().BeFalse("The customer details should be removed from the table.");
    }

    private static class CustomerFactory
    {
        public static Customer John() => new()
        {
            FirstName = "John",
            LastName = "Doe",
            BirthDate = "11/11/2000",
            Phone = "+989145799298",
            Email = "john.doe@example.com",
            BankAccount = "IR123456789"
        };

        public static Customer Jane() => new()
        {
            FirstName = "Jane",
            LastName = "Smith",
            BirthDate = "12/12/2002",
            Phone = "+989145799297",
            Email = "jane.smith@example.com",
            BankAccount = "IR987654321"
        };
    }

    public void Dispose()
    {
        _driver.Quit();
        _driver.Dispose();
    }
}