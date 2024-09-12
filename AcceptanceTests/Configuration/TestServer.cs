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
        this.server = new WebApplicationFactory<Program>().WithWebHostBuilder(builder => builder
            .ConfigureTestServices(config => config.SubstituteServices()));

        this.Client = new(this.server.CreateClient(), context, this.server.Services);
    }

    public AcceptanceClient Client { get; }

    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposedValue)
        {
            if (disposing)
            {
                this.Client.Dispose();
                this.server.Dispose();
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
