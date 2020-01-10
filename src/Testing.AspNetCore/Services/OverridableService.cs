using System.Threading.Tasks;

namespace Testing.AspNetCore.Services
{
    internal class OverridableService : IOverridableService
    {
        public Task<string> ProduceMessage()
        {
            return Task.FromResult("I am a unicorn");
        }
    }
}
