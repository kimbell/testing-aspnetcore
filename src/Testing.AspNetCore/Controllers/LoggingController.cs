using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace Testing.AspNetCore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoggingController : ControllerBase
    {
        private readonly ILogger<LoggingController> _logger;

        public LoggingController(ILogger<LoggingController> logger)
        {
            _logger = logger;
        }

        [HttpPost("message/{level}")]
        [SwaggerOperation(OperationId = "LogMessage")]
        public async Task<IActionResult> LogMessage(LogLevel level, [FromBody] string message)
        {
            _logger.Log(level, message);
            
            await Task.CompletedTask.ConfigureAwait(false);
            return Ok("It got logged");
        }
    }
}
