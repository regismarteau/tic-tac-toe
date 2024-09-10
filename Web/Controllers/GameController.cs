using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        [HttpPost("start")]
        public async Task<ActionResult<Guid>> Start()
        {
            await Task.CompletedTask;
            return this.Ok(Guid.NewGuid());
        }
    }
}
