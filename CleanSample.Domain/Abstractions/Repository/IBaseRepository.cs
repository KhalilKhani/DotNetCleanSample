namespace CleanSample.Domain.Abstractions.Repository;

public interface IBaseRepository<T> where T : class
{
    Task<T?> GetByIdAsync<TG>(TG id);

    Task<T?> GetAsync(Expression<Func<T, bool>> filter);

    Task<List<T>> GetListAsync(Expression<Func<T, bool>> filter);

    Task<IEnumerable<T>> GetAllAsync();

    Task AddAsync(T entity);

    Task AddAndSaveAsync(T entity);

    void Update(T entity);

    Task UpdateAndSaveAsync(T entity);

    void Remove(T entity);

    Task RemoveAndSaveAsync(T entity);
}