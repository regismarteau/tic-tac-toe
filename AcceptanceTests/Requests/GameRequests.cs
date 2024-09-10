using AcceptanceTests.Configuration;

namespace AcceptanceTests.Requests
{
    public class GameRequests(AcceptanceClient client)
    {
        public Task<Guid> Start()
        {
            return client.Post<Guid>("api/game/start");
        }
    }
}
