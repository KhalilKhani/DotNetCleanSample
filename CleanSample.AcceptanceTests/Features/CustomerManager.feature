Feature: Customer Manager
	As a an operator I wish to be able to Create, Update, Delete customers and list all customers
    
@create
Scenario: Create a Customer and Verify Uniqueness
	Given the customer details:
		| Firstname | Lastname | DateOfBirth | PhoneNumber   | Email                    | BankAccountNumber |
		| Khalil    | Khani    | 1990-01-01  | +989145799298 | Khalil.Khani@example.com | IR000000000000001 |
	When a request is made to create a new customer
	Then the response of creating the customer should be successful
	And the created customer details should be returned:
		| Firstname | Lastname | DateOfBirth | PhoneNumber   | Email                    | BankAccountNumber |
		| Khalil    | Khani    | 1990-01-01  | +989145799298 | Khalil.Khani@example.com | IR000000000000001 |
	When a request is made to create a new customer with the same details
	Then the response should be BadRequest due to uniqueness violation


@update
Scenario: Update a Customer and Verify Uniqueness
	Given existing customers with the following details:
		| Id | Firstname | Lastname | DateOfBirth | PhoneNumber   | Email                    | BankAccountNumber |
		| 1  | Sima      | Smith    | 1992-05-12  | +989371133325 | sima.smith@example.com   | IR000000000000001 |
		| 2  | Khalil    | Khani    | 1990-01-01  | +989145799298 | Khalil.Khani@example.com | IR000000000000002 |
	When a request is made to update customer with Id 1 and the following updated details:
		| Id | Firstname | Lastname | DateOfBirth | PhoneNumber   | Email                  | BankAccountNumber |
		| 1  | Reza      | Rezai    | 1992-06-20  | +989371133326 | reza.rezai@example.com | IR000000000000003 |
	Then the response of updating the customer should be successful
	And the updated customer details should be returned:
		| Id | Firstname | Lastname | DateOfBirth | PhoneNumber   | Email                  | BankAccountNumber |
		| 1  | Reza      | Rezai    | 1992-06-20  | +989371133326 | reza.rezai@example.com | IR000000000000003 |
	When a request is made to update customer with Id 1 and the following duplicated details:
		| Id | Firstname | Lastname | DateOfBirth | PhoneNumber   | Email                    | BankAccountNumber |
		| 1  | Khalil    | Khani    | 1990-01-01  | +989145799298 | Khalil.Khani@example.com | IR000000000000002 |
	Then the response of update should be BadRequest due to uniqueness violation


@delete
Scenario: Delete a Customer
	Given an existing customer (to be deleted) with the Id 1
	When a request is made to delete customer with Id 1
	Then the response of deleting the customer should be successful
	When a request is made to retrieve deleted customer with Id 1
	Then the response of delete should be not found


@retrieve
Scenario: Retrieve a Customer
	Given an existing customer with the following details:
		| Id | Firstname | Lastname | DateOfBirth | PhoneNumber   | Email                    | BankAccountNumber |
		| 1  | Khalil    | Khani    | 1990-01-01  | +989145799298 | Khalil.Khani@example.com | IR000000000000001 |
	When a request is made to retrieve customer with Id 1
	Then the response of retrieving the customer should be successful
	And the retrieved customer details should be returned:
		| Firstname | Lastname | DateOfBirth | PhoneNumber   | Email                    | BankAccountNumber |
		| Khalil    | Khani    | 1990-01-01  | +989145799298 | Khalil.Khani@example.com | IR000000000000001 |


@retrieveList
Scenario: List all Customers
	Given the following customers exist:
		| Id | Firstname | Lastname | DateOfBirth | PhoneNumber   | Email                     | BankAccountNumber |
		| 1  | Tom       | Hanks    | 1962-03-21  | +989371133670 | tom.hanks@example.com     | IR000000000000001 |
		| 2  | Gary      | Kasbarof | 1970-01-01  | +989145799325 | gary.kasbarof@example.com | IR000000000000002 |
	When a request is made to list all customers
	Then the response of getting all customers should be successful
	And the response should contain the following customer details:
		| Id | Firstname | Lastname | DateOfBirth | PhoneNumber   | Email                     | BankAccountNumber |
		| 1  | Tom       | Hanks    | 1962-03-21  | +989371133670 | tom.hanks@example.com     | IR000000000000001 |
		| 2  | Gary      | Kasbarof | 1970-01-01  | +989145799325 | gary.kasbarof@example.com | IR000000000000002 |