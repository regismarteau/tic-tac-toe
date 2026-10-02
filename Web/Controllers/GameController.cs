using System.ComponentModel.DataAnnotations;
using Domain.Gameplay;
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
    public Task<ActionResult<Guid>> Start(CancellationToken cancellationToken) => Dispatch(new StartAGame(), cancellationToken);

    [HttpGet("{gameId:guid}")]
    public Task<ActionResult<GameDto>> GetGameState([FromRoute] Guid gameId, CancellationToken cancellationToken) => Dispatch(new GetGameState(gameId), cancellationToken);

    [HttpPost("{gameId:guid}/play/{cell}")]
    public async Task<ActionResult> Play([FromRoute] Guid gameId, [FromRoute][EnumDataType(typeof(Cell))] Cell cell, CancellationToken cancellationToken) => await Dispatch(new Play(new(gameId), cell), cancellationToken);
}
