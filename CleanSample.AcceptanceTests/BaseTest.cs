using CleanSample.AcceptanceTests.Drivers;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace CleanSample.AcceptanceTests;

public class BaseTest : IClassFixture<WebAppFactory>, IDisposable
{
    private readonly WebAppFactory _factory;
    private readonly HttpClient _httpClient;
    protected readonly ApiClientDriver ApiClientDriver;

    public BaseTest(WebAppFactory factory)
    {
        _factory = factory;
        var clientOptions = new WebApplicationFactoryClientOptions();
        clientOptions.AllowAutoRedirect = false;

        _httpClient = factory.CreateClient(clientOptions);
        ApiClientDriver = new ApiClientDriver(_httpClient);
    }


    [BeforeScenario(Order = 0)]
    public async Task BeforeScenario()
    {
        await _factory.InitializeAsync();
    }

    [AfterScenario]
    public async Task AfterScenario()
    {
        await _factory.DisposeAsync();
    }

    public void Dispose()
    {
        _factory.Dispose();
    }
}