using Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using Queries;
using RMediator.Abstractions;
using UseCases.Commands;

namespace Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GameController(IDispatchCommand commandDispatcher, IDispatchQuery queryDispatcher) : DispatcherController(commandDispatcher, queryDispatcher)
{
    [HttpPost("start")]
    public Task<ActionResult<Guid>> Start()
    {
        return Dispatch(new StartAGame());
    }

    [HttpGet("{gameId:guid}")]
    public Task<ActionResult<GameDto>> GetGameState([FromRoute] Guid gameId)
    {
        return Dispatch(new GetGameState(gameId));
    }

    [HttpPost("{gameId:guid}/play/{cell}")]
    public async Task<ActionResult> Play([FromRoute] Guid gameId, [FromRoute] Cell cell)
    {
        return await Dispatch(new Play(new(gameId), cell));
    }
}