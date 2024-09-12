using Domain.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Queries;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using UseCases.Commands;

namespace Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GameController(IMediator mediator) : DispatcherController(mediator)
{
    [HttpPost("start")]
    public Task<ActionResult<Guid>> Start()
    {
        return this.Dispatch(new StartAGame());
    }

    [HttpGet("{gameId:guid}")]
    public Task<ActionResult<GameDto>> GetAllMarks([FromRoute] Guid gameId)
    {
        return this.Dispatch(new GetGameState(gameId));
    }

    [HttpPost("{gameId:guid}/play/{cell}")]
    public async Task<ActionResult> Play([FromRoute] Guid gameId, [FromRoute] Cell cell)
    {
        return await this.Dispatch(new Play(new(gameId), cell));
    }
}