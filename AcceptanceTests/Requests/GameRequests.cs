using AcceptanceTests.Configuration;
using Domain.Gameplay;
using Queries;

namespace AcceptanceTests.Requests;

public class GameRequests(AcceptanceClient client)
{
    public Task<Guid> Start() => client.Post<Guid>("api/game/start");

    public Task<GameDto> GetGame(Guid gameId) => client.Get<GameDto>($"api/game/{gameId}");

    public Task Play(Guid gameId, Cell cell) => client.Post($"api/game/{gameId}/play/{cell.ToString()}");
}
