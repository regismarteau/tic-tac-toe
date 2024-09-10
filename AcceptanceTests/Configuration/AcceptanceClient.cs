using System.Net.Http.Json;

namespace AcceptanceTests.Configuration;

public class AcceptanceClient(HttpClient client) : IDisposable
{
    private bool disposedValue;

    public async Task<T> Post<T>(string path)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, path);
        var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        return (await response.Content.ReadFromJsonAsync<T>()) ?? throw new InvalidOperationException("The response content is null or couldn't be deserialized.");
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
