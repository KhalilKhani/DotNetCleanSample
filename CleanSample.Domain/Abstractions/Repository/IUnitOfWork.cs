namespace CleanSample.Domain.Abstractions.Repository;

public interface IUnitOfWork
{
    public ICustomerRepository Customer { get; }

    public Task CompleteAsync();
}