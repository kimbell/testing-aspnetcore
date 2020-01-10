using System.Threading.Tasks;

namespace Testing.AspNetCore.Services
{
    /// <summary>
    /// A service that can be overriden in tests
    /// </summary>
    public interface IOverridableService
    {
        /// <summary>
        /// Produces a simple message
        /// </summary>
        /// <returns></returns>
        Task<string> ProduceMessage();
    }
}
