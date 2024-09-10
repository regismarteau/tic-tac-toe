﻿using Domain.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Queries;
using UseCases;

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
    public Task<ActionResult<MarksDto>> GetAllMarks([FromRoute] Guid gameId)
    {
        return this.Dispatch(new GetAllMarksFromGame(gameId));
    }

    [HttpPost("{gameId:guid}/play/{cell}")]
    public async Task<ActionResult> Play([FromRoute] Guid gameId, [FromRoute] Cell cell)
    {
        return await this.Dispatch(new Play(new(gameId), cell));
    }
}