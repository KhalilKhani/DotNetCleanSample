namespace CleanSample.UnitTests;

public class BaseTest : IClassFixture<WebAppFactory>, IDisposable
{
    private readonly WebAppFactory _factory;
    protected readonly HttpClient HttpClient;

    public BaseTest(WebAppFactory factory)
    {
        _factory = factory;
        var clientOptions = new WebApplicationFactoryClientOptions();
        clientOptions.AllowAutoRedirect = false;

        HttpClient = factory.CreateClient(clientOptions);
    }

    public void Dispose()
    {
        _factory.Dispose();
        HttpClient.Dispose();
    }
}