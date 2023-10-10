namespace CleanSample.UnitTests;

public class WebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MsSqlContainer _container = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
        .Build();
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {

        builder.ConfigureTestServices(services =>
        {
            var descriptor =
                services.SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
            if (descriptor is not null) services.Remove(descriptor);
            services.AddDbContext<ApplicationDbContext>(opt =>
            {
                opt.UseSqlServer(_container.GetConnectionString());
            });
        });
    }

    public Task InitializeAsync() => _container.StartAsync();

    public new Task DisposeAsync() => _container.StopAsync();
}