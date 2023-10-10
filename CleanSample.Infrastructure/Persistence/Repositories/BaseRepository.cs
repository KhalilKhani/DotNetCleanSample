namespace CleanSample.Infrastructure.Persistence.Repositories;

public abstract class BaseRepository<T> : IBaseRepository<T>
    where T : class
{
    protected readonly ApplicationDbContext DbContext;
    protected readonly DbSet<T> DbSet;

    protected BaseRepository(ApplicationDbContext dbContext)
    {
        DbContext = dbContext;
        DbSet = DbContext.Set<T>();
    }

    public async Task<T?> GetByIdAsync<TG>(TG id) => await DbSet.FindAsync(id);

    public async Task<T?> GetAsync(Expression<Func<T, bool>> filter) => await DbSet.FirstOrDefaultAsync(filter);

    public async Task<List<T>> GetListAsync(Expression<Func<T, bool>> filter) => await DbSet.Where(filter).ToListAsync();

    public async Task<IEnumerable<T>> GetAllAsync() => await DbSet.ToListAsync();

    public async Task AddAsync(T entity) => await DbSet.AddAsync(entity);

    public async Task AddAndSaveAsync(T entity)
    {
        await DbSet.AddAsync(entity);
        await DbContext.SaveChangesAsync();
    }

    public void Update(T entity) => DbContext.Entry(entity).State = EntityState.Modified;

    public async Task UpdateAndSaveAsync(T entity)
    {
        DbContext.Entry(entity).State = EntityState.Modified;
        await DbContext.SaveChangesAsync();
    }

    public void Remove(T entity) => DbContext.Set<T>().Remove(entity);

    public async Task RemoveAndSaveAsync(T entity)
    {
        DbContext.Set<T>().Remove(entity);
        await DbContext.SaveChangesAsync();
    }
}