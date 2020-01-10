using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Testing.AspNetCore.Models;
using Testing.AspNetCore.Services;

namespace Testing.AspNetCore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OverridableServiceController : ControllerBase
    {
        private readonly IOverridableService _service;

        public OverridableServiceController(IOverridableService service)
        {
            _service = service;
        }

        [HttpGet("message")]
        [SwaggerOperation(OperationId = "ProduceMessage")]
        [ProducesResponseType(typeof(OverridableServiceValue), StatusCodes.Status200OK)]
        public async Task<IActionResult> ProduceMessage()
        {
            return Ok(new OverridableServiceValue
            {
                Message = await _service.ProduceMessage().ConfigureAwait(false)
            });
        }
    }
}
