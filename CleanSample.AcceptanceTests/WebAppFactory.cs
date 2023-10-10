using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Authentication;
using CleanSample.Infrastructure.Persistence.Context;
using CleanSample.Presentation.Server;
using Testcontainers.MsSql;
using Xunit;

namespace CleanSample.AcceptanceTests;

public class WebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MsSqlContainer _container = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // builder.UseUrls("https://localhost:7045");
        builder.ConfigureTestServices(services =>
        {
            var descriptor = services.SingleOrDefault(s =>
                s.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

            if (descriptor is not null) services.Remove(descriptor);
            services.AddDbContext<ApplicationDbContext>(opt =>
            {
                opt.UseSqlServer(_container.GetConnectionString());
            });

            services.PostConfigure<KestrelServerOptions>(options =>
            {
                options.ConfigureHttpsDefaults(httpsOptions =>
                {
                    httpsOptions.SslProtocols = SslProtocols.None;
                });
            });
        });
    }

    public async Task InitializeAsync()
    {
        await _container.StartAsync();
        // Ensure the server is initialized
        // EnsureServerInitialized();

        // Create a scope to get the ApplicationDbContext
        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        // Apply the migrations
        await dbContext.Database.MigrateAsync();
    }

    private void EnsureServerInitialized()
    {
        // Send a dummy request to initialize the server
        var client = CreateClient();
        client.GetAsync("/").Wait();
    }
    public new async Task DisposeAsync()
    {
        await _container.StopAsync();
    }
}