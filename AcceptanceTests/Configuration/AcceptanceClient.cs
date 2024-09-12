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
        this.scopedServices = services.CreateScope();
    }

    private AsynchronousSideEffectsAwaiter Awaiter => this.scopedServices.ServiceProvider.GetRequiredService<AsynchronousSideEffectsAwaiter>();

    public async Task Post(string path)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, path);
        await this.SendRequest(request);
    }

    public async Task<T> Post<T>(string path)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, path);
        return await this.GetResponse<T>(request);
    }

    public async Task<T> Get<T>(string path)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, path);
        return await this.GetResponse<T>(request);
    }

    private async Task<T> GetResponse<T>(HttpRequestMessage request)
    {
        var response = await this.SendRequest(request);

        return (await response.Content.ReadFromJsonAsync<T>(new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            Converters = { new JsonStringEnumConverter() }
        })) ?? throw new InvalidOperationException("The response content is null or couldn't be deserialized.");
    }

    private async Task<HttpResponseMessage> SendRequest(HttpRequestMessage request)
    {
        var response = await this.client.SendAsync(request);
        await this.HandleError(response);
        await this.Awaiter.WaitForSideEffects();
        return response;
    }

    private async Task HandleError(HttpResponseMessage response)
    {
        if (!this.context.IsAnErrorHandlingScenario())
        {
            response.EnsureSuccessStatusCode();
            return;
        }

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadFromJsonAsync<AcceptanceError>();
            this.context.Set(error);
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposedValue)
        {
            if (disposing)
            {
                this.client.Dispose();
                this.scopedServices.Dispose();
            }

            this.disposedValue = true;
        }
    }

    public void Dispose()
    {
        this.Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
