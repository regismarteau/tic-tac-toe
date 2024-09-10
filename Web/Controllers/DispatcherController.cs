using MediatR;
using Microsoft.AspNetCore.Mvc;
using Queries;
using UseCases;

namespace Web.Controllers;

public abstract class DispatcherController(IMediator mediator) : ControllerBase
{
    public async Task<ActionResult<T>> Dispatch<T>(ICommand<T> command)
    {
        return this.Ok(await mediator.Send(command));
    }
    public async Task<ActionResult<T>> Dispatch<T>(IQuery<T> query)
    {
        return this.Ok(await mediator.Send(query));
    }
}