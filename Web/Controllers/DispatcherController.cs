using Microsoft.AspNetCore.Mvc;
using RMediator.Abstractions;

namespace Web.Controllers;

public abstract class DispatcherController(IDispatchCommand commandDispatcher, IDispatchQuery queryDispatcher) : ControllerBase
{
    protected async Task<ActionResult> Dispatch(ICommand command)
    {
        await commandDispatcher.Dispatch(command);
        return Ok();
    }

    protected async Task<ActionResult<T>> Dispatch<T>(ICommand<T> command)
    {
        return Ok(await commandDispatcher.Dispatch(command));
    }

    protected async Task<ActionResult<T>> Dispatch<T>(IQuery<T> query)
    {
        return Ok(await queryDispatcher.Dispatch(query));
    }
}