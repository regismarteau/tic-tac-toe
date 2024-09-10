using MediatR;
using Microsoft.AspNetCore.Mvc;
using UseCases;

namespace Web.Controllers;

public abstract class DispatcherController(IMediator mediator) : ControllerBase
{

    public async Task<ActionResult<T>> Dispatch<T>(ICommand<T> command)
    {
        return this.Ok(await mediator.Send(command));
    }
}