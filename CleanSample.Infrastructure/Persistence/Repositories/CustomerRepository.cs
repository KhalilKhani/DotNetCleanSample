namespace CleanSample.Infrastructure.Persistence.Repositories;

public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
{
    public CustomerRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Result<Customer>> GetByEmail(string email)
    {
        var customer = await GetAsync(c => c.Email.Value.ToLower() == email.ToLower());
        if (customer == null)
            return new Error((int)HttpStatusCode.NotFound, "Customer not found");
        return customer;
    }

    public async Task<Result<Customer>> CreateCustomer(Customer customer)
    {
        var result = await IsUniqueCustomer(customer);
        if (result.IsFailure)
            return Result<Customer>.Failure(result.Error);

        await AddAndSaveAsync(customer);
        return Result<Customer>.Success(customer);
    }

    public async Task<Result<Customer>> UpdateCustomer(Customer customer)
    {
        var result = await IsUniqueCustomer(customer);
        if (result.IsFailure)
            return Result<Customer>.Failure(result.Error);

        await UpdateAndSaveAsync(customer);
        return Result<Customer>.Success(customer);
    }

    private async Task<Result<bool>> IsUniqueCustomer(Customer customer)
    {
        var existingCustomer =
            await GetAsync(
                c => c.Id != customer.Id
                     && ((c.FirstName.Value.ToLower() == customer.FirstName.Value.ToLower()
                          && c.LastName.Value.ToLower() == customer.LastName.Value.ToLower()
                          && c.DateOfBirth.Value == customer.DateOfBirth.Value)
                         || c.Email.Value.ToLower() == customer.Email.Value.ToLower()));

        if (existingCustomer != null)
        {
            int errorCode;
            string errorMessage;
            if (existingCustomer.Email.Value.EqualsIgnoreCase(customer.Email.Value))
            {
                errorCode = (int)ErrorCodes.DuplicateCustomerByEmail;
                errorMessage = $"{customer.Email.Value} is already exists";
            }
            else
            {
                errorCode = (int)ErrorCodes.DuplicateCustomerByPersonalInfo;
                errorMessage = "A user with this name and birthdate already exists";
            }

            return new Error((int)HttpStatusCode.BadRequest, new Error(errorCode, errorMessage));
        }

        return true;
    }
}