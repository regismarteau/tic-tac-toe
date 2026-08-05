using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Reqnroll;

namespace AcceptanceTests.Configuration;

public class TestServer : IDisposable
{
    private readonly WebApplicationFactory<Program> server;
    private bool disposedValue;

    public TestServer(ScenarioContext context)
    {
        server = new WebApplicationFactory<Program>().WithWebHostBuilder(builder => builder
            .ConfigureTestServices(config => config.SubstituteServices()));

        Client = new(server.CreateClient(), context, server.Services);
    }

    public AcceptanceClient Client { get; }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                Client.Dispose();
                server.Dispose();
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
