using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Testing.AspNetCore.PetStore;
namespace Testing.AspNetCore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MockHttpController : ControllerBase
    {
        private readonly PetStoreClient _petStoreClient;

        public MockHttpController(PetStoreClient petStoreClient)
        {
            _petStoreClient = petStoreClient;
        }

        [HttpPost("add")]
        [SwaggerOperation(OperationId = "AddPet")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> AddPet()
        {
            await _petStoreClient.AddPetAsync(new Pet
            {
                Id = 1,
                Name = "Garfield",
                Status = PetStatus.Pending,
            }).ConfigureAwait(false);

            return NoContent();
        }

        [HttpGet("get")]
        [SwaggerOperation(OperationId = "GetPets")]
        [ProducesResponseType(typeof(IEnumerable<Pet>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPets()
        {
           return Ok(await _petStoreClient.FindPetsByStatusAsync(new[] { Anonymous.Available }).ConfigureAwait(false));
        }
    }
}
