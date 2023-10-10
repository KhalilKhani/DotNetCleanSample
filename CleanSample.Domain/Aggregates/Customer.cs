using CleanSample.Domain.Abstractions.Entity;
using CleanSample.Domain.Events;
using CleanSample.Domain.ValueObjects;
using CleanSample.Utility;
using DateOfBirth = CleanSample.Domain.ValueObjects.DateOfBirth;
using PhoneNumber = CleanSample.Domain.ValueObjects.PhoneNumber;

namespace CleanSample.Domain.Aggregates;

public class Customer : AggregateRoot<int>
{
    public FirstName FirstName { get; private set; }
    public LastName LastName { get; private set; }
    public DateOfBirth DateOfBirth { get; private set; }
    public PhoneNumber Phone { get; private set; }
    public Email Email { get; private set; }
    public BankAccount BankAccount { get; private set; }

    // This is needed for EF
    private Customer()
    {
    }

    private Customer(FirstName firstName, LastName lastName, DateOfBirth dateOfBirth, PhoneNumber phone, Email email, BankAccount bankAccount)
    {
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
        Phone = phone;
        Email = email;
        BankAccount = bankAccount;
    }

    public static Result<Customer> Create(string firstName, string lastName, DateTime dob, string phoneNumber,
        string email, string accountNumber)
    {
        var validationResult = ValidateCustomerInfo(firstName, lastName, dob, phoneNumber, email, accountNumber);

        if (validationResult.IsFailure)
            return validationResult.Error;

        Customer customer = new(validationResult.Value.FirstName, validationResult.Value.LastName, validationResult.Value.Dob,
            validationResult.Value.Phone, validationResult.Value.Email, validationResult.Value.BankAccount);

        customer.RaiseDomainEvent(new CustomerCreatedDomainEvent(customer));
        
        return customer;
    }

    public Result<Customer> Update(string firstName, string lastName, DateTime dob, string phoneNumber, string email,
        string accountNumber)
    {
        var validationResult = ValidateCustomerInfo(firstName, lastName, dob, phoneNumber, email, accountNumber);

        if (validationResult.IsFailure)
            return Result<Customer>.Failure(validationResult.Error);

        FirstName = validationResult.Value.FirstName;
        LastName = validationResult.Value.LastName;
        DateOfBirth = validationResult.Value.Dob;
        Phone = validationResult.Value.Phone;
        Email = validationResult.Value.Email;
        BankAccount = validationResult.Value.BankAccount;

        MarkAsUpdated();
        RaiseDomainEvent(new CustomerUpdatedDomainEvent(this));

        return Result<Customer>.Success(this);
    }

    public void MarkAsDeleted()
    {
        RaiseDomainEvent(new CustomerDeletedDomainEvent(Id));
    }
    

    private static Result<(FirstName FirstName, LastName LastName, DateOfBirth Dob, PhoneNumber Phone, Email Email, BankAccount BankAccount)> ValidateCustomerInfo(string firstName, string lastName, DateTime dob, string phoneNumber, string email,
            string accountNumber)
    {
        var firstNameResult = FirstName.Create(firstName);
        if (firstNameResult.IsFailure) return firstNameResult.Error;
        
        var lastNameResult = LastName.Create(lastName);
        if (lastNameResult.IsFailure)
            return lastNameResult.Error;
        var dobResult = DateOfBirth.Create(dob);
        if (dobResult.IsFailure)
            return dobResult.Error;
        var phoneResult = PhoneNumber.Create(phoneNumber);
        if (phoneResult.IsFailure)
            return phoneResult.Error;
        var emailResult = Email.Create(email);
        if (emailResult.IsFailure)
            return emailResult.Error;
        var bankAccountResult = BankAccount.Create(accountNumber);
        if (bankAccountResult.IsFailure)
            return bankAccountResult.Error;
        
        return (firstNameResult.Value, lastNameResult.Value, dobResult.Value, phoneResult.Value, emailResult.Value, bankAccountResult.Value);
    }
}