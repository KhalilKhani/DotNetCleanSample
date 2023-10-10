namespace CleanSample.Infrastructure;

public static class InfrastructureDi
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        var isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
        var isTestMode = Environment.GetEnvironmentVariable("IS_TEST_MODE") == "true";

        if (isDevelopment && isTestMode)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("TestDb"));
        }
        else
        {
            services.AddDbContext<ApplicationDbContext>(opt => opt.UseSqlServer(connectionString));
        }

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}