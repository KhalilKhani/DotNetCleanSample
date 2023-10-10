namespace CleanSample.Utility;

public class HttpApiClient
{
    private readonly HttpClient _client;
    private readonly AsyncRetryPolicy _retryPolicy;

    public HttpApiClient(string baseAddress)
    {
        _client = new HttpClient { BaseAddress = new Uri(baseAddress) };
        _retryPolicy = Policy.Handle<HttpRequestException>()
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                (_, _, _) => { Console.WriteLine("Log exception here"); });
    }

    public HttpApiClient(HttpClient httpClient)
    {
        _client = httpClient;
        _retryPolicy = Policy.Handle<HttpRequestException>()
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                (_, _, _) => { Console.WriteLine("Log exception here"); });
    }

    public async Task<Result<T>> SendRequestAsync<T>(HttpMethod method, string uri, object? value = null)
    {
        var response = await _retryPolicy.ExecuteAsync(() =>
        {
            HttpRequestMessage request = new(method, uri);
            if (value != null)
            {

                var json = JsonConvert.SerializeObject(value);
                request.Content = new StringContent(json, Encoding.UTF8,
                    "application/json");
            }

            return _client.SendAsync(request);
        });

        return await HandleResponse<T>(response);
    }

    public async Task<Result> SendRequestAsync(HttpMethod method, string uri, object? value = null)
    {
        var response = await _retryPolicy.ExecuteAsync(() =>
        {
            HttpRequestMessage request = new(method, uri);
            if (value != null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(value), Encoding.UTF8,
                    "application/json");
            }

            return _client.SendAsync(request);
        });

        return await HandleResponse(response);
    }

    private static async Task<Result> HandleResponse(HttpResponseMessage response)
    {
        return !response.IsSuccessStatusCode
            ? Result.Failure(await ExtractErrorAsync(response))
            : Result.Success();
    }

    private static async Task<Error> ExtractErrorAsync(HttpResponseMessage response)
    {
        object? innerError;
        try
        {
            innerError = await response.Content.ReadFromJsonAsync<Error>();
        }
        catch
        {
            innerError = response.ReasonPhrase;
        }
        return new Error((int)response.StatusCode, innerError);
    }


    private static async Task<Result<T>> HandleResponse<T>(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
            return Result<T>.Failure(new Error((int)response.StatusCode, response.ReasonPhrase));

        var content = await response.Content.ReadAsStringAsync();
        var value = JsonConvert.DeserializeObject<T>(content);
        if (value == null)
            return Result<T>.Failure(new Error((int)HttpStatusCode.InternalServerError, "Something went wrong!"));

        return Result<T>.Success(value);
    }
}