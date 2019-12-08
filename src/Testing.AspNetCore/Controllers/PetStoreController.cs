using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
//using Swashbuckle.AspNetCore.Annotations;
using Testing.AspNetCore.PetStore;

namespace Testing.AspNetCore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PetStoreController : ControllerBase
    {
        private readonly PetStoreClient _client;

        public PetStoreController(PetStoreClient client)
        {
            _client = client;
        }

        //[HttpGet]
        //[SwaggerOperation(OperationId = "GetPets")]
        //public Task<IActionResult> GetPets()
        //{
           
        //}
    }
}
