using Microsoft.AspNetCore.Mvc;
using RMediator.Abstractions;

namespace Web.Controllers;

public abstract class DispatcherController(IDispatchCommand commandDispatcher, IDispatchQuery queryDispatcher) : ControllerBase
{
    protected async Task<ActionResult> Dispatch(ICommand command, CancellationToken cancellationToken)
    {
        await commandDispatcher.Dispatch(command, cancellationToken);
        return Ok();
    }

    protected async Task<ActionResult<T>> Dispatch<T>(ICommand<T> command, CancellationToken cancellationToken)
    {
        return Ok(await commandDispatcher.Dispatch(command, cancellationToken));
    }

    protected async Task<ActionResult<T>> Dispatch<T>(IQuery<T> query, CancellationToken cancellationToken)
    {
        return Ok(await queryDispatcher.Dispatch(query, cancellationToken));
    }
}
