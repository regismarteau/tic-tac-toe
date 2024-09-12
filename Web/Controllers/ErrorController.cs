using Domain.Exceptions;
using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Web.Controllers
{
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : ControllerBase
    {
        [Route("error")]
        public IActionResult HandleError()
        {
            var handler = this.HttpContext.Features.Get<IExceptionHandlerFeature>()!;
            return this.Problem(
                statusCode: (int)GetCode(handler.Error),
                detail: handler.Error.Message,
                title: handler.Error.GetType().Name);
        }

        private static HttpStatusCode GetCode(Exception exception)
        {
            return exception switch
            {
                DomainException => HttpStatusCode.BadRequest,
                NotFoundException => HttpStatusCode.NotFound,
                _ => HttpStatusCode.InternalServerError,
            };
        }
    }
}
