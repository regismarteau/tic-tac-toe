using AcceptanceTests.Configuration;
using Queries;

namespace AcceptanceTests.Requests
{
    public class GameRequests(AcceptanceClient client)
    {
        public Task<Guid> Start()
        {
            return client.Post<Guid>("api/game/start");
        }

        public Task<MarksDto> GetAllMarks(Guid gameId)
        {
            return client.Get<MarksDto>($"api/game/{gameId}");
        }
    }
}
