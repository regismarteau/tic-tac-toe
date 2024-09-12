using MediatR;
using Microsoft.AspNetCore.Mvc;
using Queries;
using UseCases.Commands;

namespace Web.Controllers;

public abstract class DispatcherController(IMediator mediator) : ControllerBase
{
    protected async Task<ActionResult> Dispatch(ICommand command)
    {
        await mediator.Send(command);
        return this.Ok();
    }

    protected async Task<ActionResult<T>> Dispatch<T>(ICommand<T> command)
    {
        return this.Ok(await mediator.Send(command));
    }

    protected async Task<ActionResult<T>> Dispatch<T>(IQuery<T> query)
    {
        return this.Ok(await mediator.Send(query));
    }
}