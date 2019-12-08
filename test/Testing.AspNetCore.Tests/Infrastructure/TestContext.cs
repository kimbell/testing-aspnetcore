using System;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RichardSzalay.MockHttp;
using Xunit;
using Xunit.Abstractions;

namespace Testing.AspNetCore.Tests.Infrastructure
{
    /// <summary>
    /// The test context captures information for a single test
    /// </summary>
    public class TestContext : WebApplicationFactory<Startup>
    {
        private readonly TestOutputLoggerProvider _loggerProvider;

        private HttpClient _httpClient;
        private ApiClient _apiClient;
        
        /// <summary>
        /// Gets the <see cref="HttpClient"/> used for testing our API
        /// </summary>
        public HttpClient HttpClient
        {
            get
            {
                if (_httpClient != null)
                {
                    return _httpClient;
                }

                _httpClient = CreateClient();
                return _httpClient;
            }
        }

        /// <summary>
        /// Allows for test specific changes to the <see cref="IServiceCollection"/>
        /// </summary>
        public Action<WebHostBuilderContext, IServiceCollection> AdditionalServices { private get; set; }

        /// <summary>
        /// Allows for test specific changes to the <see cref="IConfigurationBuilder"/>
        /// </summary>
        public Action<WebHostBuilderContext, IConfigurationBuilder> AdditionalConfiguration { private get; set; }

        /// <summary>
        /// Allows for test specific changes to the <see cref="ILoggingBuilder"/>
        /// </summary>
        public Action<WebHostBuilderContext, ILoggingBuilder> AdditionalLogging { private get; set; }

        /// <summary>
        /// A handler to mock outgoing PetStore requests
        /// </summary>
        public MockHttpMessageHandler PetStoreMockHandler { get; } = new MockHttpMessageHandler();

        public TestContext(ITestOutputHelper output)
        {
            _loggerProvider = new TestOutputLoggerProvider(output);
        }

        /// <summary>
        /// A strongly typed client for our API
        /// </summary>
        public ApiClient ApiClient
        {
            get
            {
                if (_apiClient != null)
                {
                    return _apiClient;
                }

                return _apiClient = new ApiClient("http://api", HttpClient);
            }
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder
                .UseEnvironment("BuildTest")
                .UseDefaultServiceProvider(p => p.ValidateScopes = true)
                .ConfigureServices((context, services) =>
                {    
                    AdditionalServices?.Invoke(context, services);
                })
                .ConfigureAppConfiguration((context, configurationBuilder) =>
                {
                    AdditionalConfiguration?.Invoke(context, configurationBuilder);
                })
                .ConfigureLogging((context, builder) =>
                {
                    builder.ClearProviders();
                    // values are picked up from appsetting.json
                    builder.AddProvider(_loggerProvider);
                    
                    // log level cannot be modified here since this is triggered after the configuration settings have been established
                    // need to make changes in the AdditionalConfiguration since that happens earlier in the lifecycle
                    AdditionalLogging?.Invoke(context, builder);
                });
        }

        public void VerifyLog(LogLevel logLevel, string message = null, string category = null, int? count = null)
        {
            var byLogLevel = _loggerProvider.Entries.Where(l => l.LogLevel == logLevel);

            if (string.IsNullOrEmpty(message) == false)
            {
                byLogLevel = byLogLevel.Where(l => l.Message.Contains(message));
            }

            if (string.IsNullOrEmpty(category) == false)
            {
                byLogLevel = byLogLevel.Where(l => l.Category.Contains(category));
            }

            if (count == null)
            {
                Assert.NotNull(byLogLevel.FirstOrDefault());
            }
            else
            {
                Assert.Equal(count, byLogLevel.Count());
            }
        }
    }
}