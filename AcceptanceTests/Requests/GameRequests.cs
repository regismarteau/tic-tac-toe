using AcceptanceTests.Configuration;
using Domain.ValueObjects;
using Queries;

namespace AcceptanceTests.Requests
{
    public class GameRequests(AcceptanceClient client)
    {
        public Task<Guid> Start()
        {
            return client.Post<Guid>("api/game/start");
        }

        public Task<GameDto> GetGame(Guid gameId)
        {
            return client.Get<GameDto>($"api/game/{gameId}");
        }

        public Task Play(Guid gameId, Cell cell)
        {
            return client.Post($"api/game/{gameId}/play/{cell.ToString()}");
        }
    }
}
