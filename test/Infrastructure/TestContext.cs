using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RichardSzalay.MockHttp;
using Testing.AspNetCore.Tests.External;
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
        public Action<IServiceCollection> AdditionalServices { private get; set; }

        /// <summary>
        /// Allows for test specific changes to the <see cref="IConfigurationBuilder"/>
        /// </summary>
        public Action<IConfigurationBuilder> AdditionalConfiguration { private get; set; }

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
                    //services.PostConfigure<LoggerFilterOptions>(options =>
                    //{
                    //    options.MinLevel = LogLevel.Trace;
                    //    // set Microsoft category to a higher level so that we don't get logging noise in our output
                    //    options.AddFilter("Microsoft", LogLevel.Warning);
                    //});
                 
                    AdditionalServices?.Invoke(services);
                })
                .ConfigureAppConfiguration(configurationBuilder =>
                {
                    configurationBuilder.AddInMemoryCollection(new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                    {
                        // for testing, we turn on all logging
                        {"Logging:LogLevel:Default", "Trace"},
                        // set Microsoft category to a higher level so that we don't get logging noise in our output
                        {"Logging:LogLevel:Microsoft", "Warning"}
                    });

                    AdditionalConfiguration?.Invoke(configurationBuilder);
                })
                .ConfigureLogging(l =>
                {
                    l.ClearProviders();
                    l.AddProvider(_loggerProvider);
                });
        }

        public void VerifyMessageAsLogged(LogLevel logLevel, string message, string category = null)
        {
            var e = category == null
                ? _loggerProvider.Entries.FirstOrDefault(entry =>
                    entry.LogLevel == logLevel &&
                    entry.Message == message)
                : _loggerProvider.Entries.FirstOrDefault(entry =>
                    entry.LogLevel == logLevel &&
                    entry.Message == message &&
                    entry.Category == category);

            Assert.NotNull(e);
        }

        public void VerifyMessageAsNotLogged(LogLevel logLevel, string message, string category = null)
        {
            var e = category == null
                ? _loggerProvider.Entries.FirstOrDefault(entry =>
                    entry.LogLevel == logLevel &&
                    entry.Message == message)
                : _loggerProvider.Entries.FirstOrDefault(entry =>
                    entry.LogLevel == logLevel &&
                    entry.Message == message &&
                    entry.Category == category);

            Assert.Null(e);
        }
    }
}