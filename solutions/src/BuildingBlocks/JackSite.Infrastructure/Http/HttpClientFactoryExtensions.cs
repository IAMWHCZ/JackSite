namespace JackSite.Infrastructure.Http;

public static class HttpClientFactoryExtensions
{
    public static IServiceCollection AddDefaultHttpClient(
        this IServiceCollection services,
        string name,
        string baseAddress)
    {
        services.AddHttpClient(name, client =>
        {
            client.BaseAddress = new Uri(baseAddress);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        })
        .AddPolicyHandler(RetryPolicy)
        .AddPolicyHandler(CircuitBreakerPolicy);

        return services;
    }

    private static IAsyncPolicy<HttpResponseMessage> RetryPolicy => HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(3, retryAttempt =>
                TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

    private static IAsyncPolicy<HttpResponseMessage> CircuitBreakerPolicy => HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
}