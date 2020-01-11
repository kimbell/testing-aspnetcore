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
            using (var tc = new TestContext(_output))
            {
                var client = tc.HttpClient; // required to initialize things
                var options = tc.Server.Services.GetRequiredService<IOptions<GenericOption>>();

                Assert.Equal("From config file", options.Value.Value);

                await Task.CompletedTask;
            }
        }

        /// <summary>
        /// Reads the value that was set using a configuration provider
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task ChangedUsingConfiguration()
        {
            using (var tc = new TestContext(_output))
            {
                tc.AdditionalConfiguration = (context, configuration) =>
                {
                    configuration.AddInMemoryCollection(new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                    {
                        {"Generic:Value", "From configuration provider"}
                    });
                };
               
                var client = tc.HttpClient; // required to initialize things
                var options = tc.Server.Services.GetRequiredService<IOptions<GenericOption>>();

                Assert.Equal("From configuration provider", options.Value.Value);

                await Task.CompletedTask;
            }
        }

        /// <summary>
        /// Reads the value that was configured using PostConfigure
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task ChangedUsingPostConfigure()
        {
            using (var tc = new TestContext(_output))
            {
                tc.AdditionalServices = (context, services) =>
                {
                    services.PostConfigure<GenericOption>(o => { o.Value = "From post configure"; });
                };            

                var client = tc.HttpClient; // required to initialize things
                var options = tc.Server.Services.GetRequiredService<IOptions<GenericOption>>();

                Assert.Equal("From post configure", options.Value.Value);

                await Task.CompletedTask;
            }
        }
    }
}
