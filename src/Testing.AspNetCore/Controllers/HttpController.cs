using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Testing.AspNetCore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HttpController : ControllerBase
    {
        [HttpPost("headers")]
        [SwaggerOperation(OperationId = "GetHeaders")]
        [ProducesResponseType(typeof(Dictionary<string, string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetHeaders()
        {
            await Task.CompletedTask.ConfigureAwait(false);

            // this method just returns the headers being received. 
            // In real logic you would do something with the values
            var headers = HttpContext.Request.Headers;
                       
            return Ok(headers.ToDictionary(x => x.Key, x => x.Value.ToString()));
        }

        [HttpPost("cookies")]
        [SwaggerOperation(OperationId = "GetCookies")]
        [ProducesResponseType(typeof(Dictionary<string, string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCookies()
        {
            await Task.CompletedTask.ConfigureAwait(false);

            // this method just returns the cookies being received.
            // In real logic you would do something with the values
            var cookies = HttpContext.Request.Cookies;

            return Ok(cookies.ToDictionary(x => x.Key, x => x.Value));
        }
    }
}
