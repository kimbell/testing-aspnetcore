using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Testing.AspNetCore.Services;
using Testing.AspNetCore.Tests.Infrastructure;
using Xunit;
using Xunit.Abstractions;

namespace Testing.AspNetCore.Tests
{
    public class OverridableServiceTests
    {
        private readonly ITestOutputHelper _output;

        public OverridableServiceTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task UseOriginalImplemenation()
        {
            using (var tc = new TestContext(_output))
            {
                var response = await tc.ApiClient.ProduceMessageAsync().ConfigureAwait(false);

                Assert.Equal("I am a unicorn", response.Message);
            }
        }

        [Fact]
        public async Task UseOverridenImplemenation()
        {
            using (var tc = new TestContext(_output))
            {
                tc.AdditionalServices = (_, services) =>
                {
                    var mockService = new Mock<IOverridableService>();
                    mockService.Setup(x => x.ProduceMessage()).ReturnsAsync("I'm a pony with a party hat");

                    // since the original service was registered with TryAddSingleton(), we can replace it. 
                    services.AddSingleton(mockService.Object);
                };

                var response = await tc.ApiClient.ProduceMessageAsync().ConfigureAwait(false);

                Assert.Equal("I'm a pony with a party hat", response.Message);
            }
        }
    }
}
