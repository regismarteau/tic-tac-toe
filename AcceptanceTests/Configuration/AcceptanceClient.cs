using System.Net.Http.Json;

namespace AcceptanceTests.Configuration;

public class AcceptanceClient(HttpClient client) : IDisposable
{
    private bool disposedValue;

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
        var response = await SendRequest(request);

        return (await response.Content.ReadFromJsonAsync<T>()) ?? throw new InvalidOperationException("The response content is null or couldn't be deserialized.");
    }

    private async Task<HttpResponseMessage> SendRequest(HttpRequestMessage request)
    {
        var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        return response;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposedValue)
        {
            if (disposing)
            {
                client.Dispose();
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
