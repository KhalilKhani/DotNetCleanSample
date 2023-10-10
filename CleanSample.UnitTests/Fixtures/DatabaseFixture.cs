namespace CleanSample.UnitTests.Fixtures;

public class DatabaseFixture : IDisposable
{
    public ApplicationDbContext Context { get; }

    public DatabaseFixture()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        Context = new ApplicationDbContext(options);

        // We can seed the database with some test data
        // Context.Customers.Add(new Customer(...));
        // Context.SaveChanges();
    }

    public void Dispose()
    {
        Context.Dispose();
    }
}