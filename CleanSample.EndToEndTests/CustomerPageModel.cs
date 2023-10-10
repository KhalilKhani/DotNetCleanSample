using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace CleanSample.EndToEndTests;

public class CustomerPageModel
{
    private readonly IWebDriver _driver;
    private readonly WebDriverWait _wait;
    private const string Url = "https://localhost:7001";

    public CustomerPageModel(IWebDriver driver)
    {
        _driver = driver;
        _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
    }

    // Locators
    private By AddCustomerButton => By.Id("AddCustomerButton");
    private By FirstNameInput => By.Id("firstname");
    private By LastNameInput => By.Id("lastname");
    private By BirthDateInput => By.Id("birthdate");
    private By PhoneInput => By.Id("phone");
    private By EmailInput => By.Id("email");
    private By BankAccountInput => By.Id("bankAccount");
    private By ModalSubmitButton => By.Id("ModalSubmitButton");
    private By CustomersTableRows => By.CssSelector("#CustomersTable tbody tr");
    private By FirstCustomerRow => By.CssSelector("#CustomersTable tbody tr:first-child");
    private By UpdateButton => By.CssSelector(".update");
    private By DeleteButton => By.CssSelector(".delete");

    public void GoTo()
    {
        _driver.Navigate().GoToUrl(Url);
    }

    public void AddCustomer(Customer customer)
    {
        _driver.FindElement(AddCustomerButton).Click();
        FillCustomerForm(customer);
        _driver.FindElement(ModalSubmitButton).Click();
    }

    public void UpdateCustomer(Customer updatedCustomer)
    {
        _driver.FindElement(FirstCustomerRow).FindElement(UpdateButton).Click();
        FillCustomerForm(updatedCustomer);
        _driver.FindElement(ModalSubmitButton).Click();
    }

    public void DeleteFirstCustomer()
    {
        _driver.FindElement(FirstCustomerRow).FindElement(DeleteButton).Click();
    }

    public void WaitForModalToClose()
    {
        _wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.Id("customerModal")));
    }

    public bool IsCustomerInTable(Customer customer)
    {
        var rows = _driver.FindElements(CustomersTableRows);
        return rows.Any(r => r.Text.Contains(customer.FirstName)
                             && r.Text.Contains(customer.LastName)
                             && r.Text.Contains(customer.BirthDate)
                             && r.Text.Contains(customer.Email)
                             && r.Text.Contains(customer.Phone)
                             && r.Text.Contains(customer.BankAccount));
    }


    private void FillCustomerForm(Customer customer)
    {
        var firstNameElement = _driver.FindElement(FirstNameInput);
        firstNameElement.Clear();
        firstNameElement.SendKeys(customer.FirstName);

        var lastNameElement = _driver.FindElement(LastNameInput);
        lastNameElement.Clear();
        lastNameElement.SendKeys(customer.LastName);

        var birthDateElement = _driver.FindElement(BirthDateInput);
        birthDateElement.Clear();
        birthDateElement.SendKeys(customer.BirthDate);

        var phoneElement = _driver.FindElement(PhoneInput);
        phoneElement.Clear();
        phoneElement.SendKeys(customer.Phone);

        var emailElement = _driver.FindElement(EmailInput);
        emailElement.Clear();
        emailElement.SendKeys(customer.Email);

        var bankAccountElement = _driver.FindElement(BankAccountInput);
        bankAccountElement.Clear();
        bankAccountElement.SendKeys(customer.BankAccount);
    }

}

public class Customer
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string BirthDate { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string BankAccount { get; set; }
}