using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Testing.AspNetCore.PetStore;
using Testing.AspNetCore.Tests.Infrastructure;
using Xunit;
using Xunit.Abstractions;

namespace Testing.AspNetCore.Tests
{
    /// <summary>
    /// Tests for manipulating option values for testing purposes
    /// </summary>
    public class OptionTests
    {
        private readonly ITestOutputHelper _output;

        public OptionTests(ITestOutputHelper output)
        {
            _output = output;
        }

        /// <summary>
        /// Reads the original value as specified in the appsettings.json
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task ReadOriginalValue()
        {
            using var tc = new TestContext(_output);

            var client = tc.HttpClient; // required to initialize things
            var options = tc.Server.Services.GetRequiredService<IOptions<PetStoreOptions>>();

            Assert.Equal("https://petstore.swagger.io/v2", options.Value.Url);

            await Task.CompletedTask;
        }

        /// <summary>
        /// Reads the value that was set using a configuration provider
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task ChangedUsingConfiguration()
        {
            using var tc = new TestContext(_output)
            {
                AdditionalConfiguration = configuration =>
                {
                    configuration.AddInMemoryCollection(new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                    {
                        {"PetStore:Url", "http://storepet"}
                    });
                }
            };

            var client = tc.HttpClient; // required to initialize things
            var options = tc.Server.Services.GetRequiredService<IOptions<PetStoreOptions>>();

            Assert.Equal("http://storepet", options.Value.Url);

            await Task.CompletedTask;
        }

        /// <summary>
        /// Reads the value that was configured using PostConfigure
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task ChangedUsingPostConfigure()
        {
            using var tc = new TestContext(_output)
            {
                AdditionalServices = services =>
                {
                    services.PostConfigure<PetStoreOptions>(o => { o.Url = "http://petstore"; });
                }
            };
            
            var client = tc.HttpClient; // required to initialize things
            var options = tc.Server.Services.GetRequiredService<IOptions<PetStoreOptions>>();

            Assert.Equal("http://petstore", options.Value.Url);

            await Task.CompletedTask;
        }
    }
}
