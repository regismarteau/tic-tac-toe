using AcceptanceTests.Configuration;
using AcceptanceTests.Requests;
using Reqnroll;

namespace AcceptanceTests.Steps;

[Binding]
public class BaseSteps(ScenarioContext context)
{
    protected ScenarioContext Context { get; } = context;

    protected GameRequests GameRequests => new(Client);

    private AcceptanceClient Client => Context.Get<TestServer>().Client;
}