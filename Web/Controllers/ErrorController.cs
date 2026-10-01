using Database.Exceptions;
using Domain;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
public class ErrorController(ILogger<ErrorController> logger) : ControllerBase
{
    [Route("error")]
    public IActionResult HandleError()
    {
        var exception = HttpContext.Features.Get<IExceptionHandlerFeature>()!.Error;

        return exception switch
        {
            DomainException => Problem(StatusCodes.Status400BadRequest, exception.Message),
            NotFoundException => Problem(StatusCodes.Status404NotFound, exception.Message),
            _ => UnexpectedError(exception)
        };
    }

    private ObjectResult UnexpectedError(Exception exception)
    {
        logger.LogError(exception, "Unexpected error while handling {Method} {Path}", Request.Method, Request.Path);
        return Problem(StatusCodes.Status500InternalServerError);
    }

    private ObjectResult Problem(int statusCode, string? detail = null)
    {
        return base.Problem(statusCode: statusCode, detail: detail);
    }
}
