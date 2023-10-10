namespace CleanSample.Infrastructure.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        Customer = new CustomerRepository(context);
    }
        
    public ICustomerRepository Customer { get; }

    public Task CompleteAsync() => _context.SaveChangesAsync();
}