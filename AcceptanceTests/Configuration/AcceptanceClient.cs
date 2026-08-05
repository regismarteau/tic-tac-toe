using AcceptanceTests.ErrorHandling;
using Microsoft.Extensions.DependencyInjection;
using Reqnroll;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AcceptanceTests.Configuration;

public class AcceptanceClient : IDisposable
{
    private bool disposedValue;
    private readonly HttpClient client;
    private readonly ScenarioContext context;
    private readonly IServiceScope scopedServices;

    public AcceptanceClient(HttpClient client, ScenarioContext context, IServiceProvider services)
    {
        this.client = client;
        this.context = context;
        scopedServices = services.CreateScope();
    }

    private AsynchronousSideEffectsAwaiter Awaiter => scopedServices.ServiceProvider.GetRequiredService<AsynchronousSideEffectsAwaiter>();

    public async Task Post(string path)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, path);
        await SendRequest(request);
    }

    public async Task<T> Post<T>(string path)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, path);
        return await GetResponse<T>(request);
    }

    public async Task<T> Get<T>(string path)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, path);
        return await GetResponse<T>(request);
    }

    private async Task<T> GetResponse<T>(HttpRequestMessage request)
    {
        var response = await SendRequest(request);

        return (await response.Content.ReadFromJsonAsync<T>(new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            Converters = { new JsonStringEnumConverter() }
        })) ?? throw new InvalidOperationException("The response content is null or couldn't be deserialized.");
    }

    private async Task<HttpResponseMessage> SendRequest(HttpRequestMessage request)
    {
        var response = await client.SendAsync(request);
        await HandleError(response);
        await Awaiter.WaitForSideEffects();
        return response;
    }

    private async Task HandleError(HttpResponseMessage response)
    {
        if (!context.IsAnErrorHandlingScenario())
        {
            response.EnsureSuccessStatusCode();
            return;
        }

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadFromJsonAsync<AcceptanceError>();
            context.Set(error);
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                client.Dispose();
                scopedServices.Dispose();
            }

            disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
