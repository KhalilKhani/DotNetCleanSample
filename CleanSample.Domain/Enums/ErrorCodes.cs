namespace CleanSample.Domain.Enums;

public enum ErrorCodes
{
    //Stateless
    InvalidMobileNumber = 101,
    InvalidEmailAddress = 102,
    InvalidBankAccountNumber = 103,
    InvalidFirstName = 104,
    InvalidLastName = 105,
    InvalidBirthDate = 106,

    //Stateful
    DuplicateCustomerByPersonalInfo = 201,
    DuplicateCustomerByEmail = 202
}