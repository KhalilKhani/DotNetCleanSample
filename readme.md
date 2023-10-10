# .NET CRUD SAMPLE

This is an ASP.NET WebAPI project that implements a CRUD application using  
Domain-Driven Design (**DDD**),  
Behavior-Driven Development (**BDD**),  
Test-Driven Development (**TDD**),  
And Command Query Responsibility Segregation (**CQRS**) principles.  

It uses  
**SpecFlow** for BDD,  
**Selenuim** for end to end tests,  
**xUnit** for unit tests,  
And **TestContainers** for throwaway instances of DataBase,

## Model

The project uses the following model:

```csharp
public class Customer {
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string BankAccount { get; private set; }
}
```

## Database  
This project uses **MSSQL Server**. You can update the connection string in the **appsettings.json** file located in the default project.

## TestMode Launch Profile  
A launch profile named **"TestMode"** has been created for running **end to end** tests (this will replace with TestContainers in the future).  
In TestMode profile, an in-memory database is used, which gets reset for each run.  
**Note**: You should run the project in test mode before running end to end tests.  

By default, the project runs on port 5001 for HTTPS and port 5000 for HTTP.  
However, when running in TestMode, it runs on port 7001 for HTTPS and port 7000 for HTTP.  

## Acceptance Tests  
For the acceptance tests, you don't need to start the API project manually. When you run the tests, the API project will automatically spin up for each scenario.
**Note**: As we use TestContainers in acceptance tests, make sure the docker (docker desktop in windows) is up and running on your machine. 


For any further information or assistance, please contact me via **khalil.khani2020@gmail.com**
