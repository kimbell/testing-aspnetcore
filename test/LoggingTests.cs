using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Testing.AspNetCore.Tests.External;
using Testing.AspNetCore.Tests.Infrastructure;
using Xunit;
using Xunit.Abstractions;

namespace Testing.AspNetCore.Tests
{
    /// <summary>
    /// Tests to show how one can check that logging has been performed
    /// </summary>
    public class LoggingTests
    {
        private readonly ITestOutputHelper _output;

        public LoggingTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task TraceMessageLogged()
        {
            const string message = "This is a trace message";
            const string category = "Testing.AspNetCore.Controllers.LoggingController";
            using var tc = new TestContext(_output);

            await tc.ApiClient.LogMessageAsync(Level.Trace, message);

            tc.VerifyMessageAsLogged(LogLevel.Trace, message, category);
        }

        [Fact]
        public async Task TraceMessageNotLogged()
        {
            const string message = "This is a trace message";
            using var tc = new TestContext(_output)
            {
                AdditionalConfiguration = configuration =>
                {
                    configuration.AddInMemoryCollection(new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                    {
                        {"Logging:LogLevel:Default", "None"}
                    });
                }
            };

            await tc.ApiClient.LogMessageAsync(Level.Trace, message);

            tc.VerifyMessageAsNotLogged(LogLevel.Trace, message);
        }

        [Fact]
        public async Task DebugMessageLogged()
        {
            const string message = "This is a debug message";
            const string category = "Testing.AspNetCore.Controllers.LoggingController";
            using var tc = new TestContext(_output);

            await tc.ApiClient.LogMessageAsync(Level.Debug, message);

            tc.VerifyMessageAsLogged(LogLevel.Debug, message,category);
        }

        [Fact]
        public async Task DebugMessageNotLogged()
        {
            const string message = "This is a debug message";
            using var tc = new TestContext(_output)
            {
                AdditionalConfiguration = configuration =>
                {
                    configuration.AddInMemoryCollection(new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                    {
                        {"Logging:LogLevel:Default", "None"}
                    });
                }
            };

            await tc.ApiClient.LogMessageAsync(Level.Debug, message);

            tc.VerifyMessageAsNotLogged(LogLevel.Debug, message);
        }

        [Fact]
        public async Task InformationMessageLogged()
        {
            const string message = "This is a information message";
            const string category = "Testing.AspNetCore.Controllers.LoggingController";
            using var tc = new TestContext(_output);

            await tc.ApiClient.LogMessageAsync(Level.Information, message);

            tc.VerifyMessageAsLogged(LogLevel.Information, message, category);
        }

        [Fact]
        public async Task InformationMessageNotLogged()
        {
            const string message = "This is a information message";
            using var tc = new TestContext(_output)
            {
                AdditionalConfiguration = configuration =>
                {
                    configuration.AddInMemoryCollection(new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                    {
                        {"Logging:LogLevel:Default", "None"}
                    });
                }
            };

            await tc.ApiClient.LogMessageAsync(Level.Information, message);

            tc.VerifyMessageAsNotLogged(LogLevel.Information, message);
        }


        [Fact]
        public async Task WarningMessageLogged()
        {
            const string message = "This is a warning message";
            const string category = "Testing.AspNetCore.Controllers.LoggingController";
            using var tc = new TestContext(_output);

            await tc.ApiClient.LogMessageAsync(Level.Warning, message);

            tc.VerifyMessageAsLogged(LogLevel.Warning, message, category);
        }

        [Fact]
        public async Task WarningMessageNotLogged()
        {
            const string message = "This is a warning message";
            using var tc = new TestContext(_output)
            {
                AdditionalConfiguration = configuration =>
                {
                    configuration.AddInMemoryCollection(new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                    {
                        {"Logging:LogLevel:Default", "None"}
                    });
                }
            };

            await tc.ApiClient.LogMessageAsync(Level.Warning, message);

            tc.VerifyMessageAsNotLogged(LogLevel.Warning, message);
        }

        [Fact]
        public async Task ErrorMessageLogged()
        {
            const string message = "This is a error message";
            const string category = "Testing.AspNetCore.Controllers.LoggingController";
            using var tc = new TestContext(_output);

            await tc.ApiClient.LogMessageAsync(Level.Error, message);

            tc.VerifyMessageAsLogged(LogLevel.Error, message, category);
        }

        [Fact]
        public async Task ErrorMessageNotLogged()
        {
            const string message = "This is a error message";
            using var tc = new TestContext(_output)
            {
                AdditionalConfiguration = configuration =>
                {
                    configuration.AddInMemoryCollection(new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                    {
                        {"Logging:LogLevel:Default", "None"}
                    });
                }
            };

            await tc.ApiClient.LogMessageAsync(Level.Error, message);

            tc.VerifyMessageAsNotLogged(LogLevel.Error, message);
        }

        [Fact]
        public async Task CriticalMessageLogged()
        {
            const string message = "This is a critical message";
            const string category = "Testing.AspNetCore.Controllers.LoggingController";
            using var tc = new TestContext(_output);

            await tc.ApiClient.LogMessageAsync(Level.Critical, message);

            tc.VerifyMessageAsLogged(LogLevel.Critical, message, category);
        }

        [Fact]
        public async Task CriticalMessageNotLogged()
        {
            const string message = "This is a critical message";
            using var tc = new TestContext(_output)
            {
                AdditionalConfiguration = configuration =>
                {
                    configuration.AddInMemoryCollection(new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                    {
                        {"Logging:LogLevel:Default", "None"}
                    });
                }
            };

            await tc.ApiClient.LogMessageAsync(Level.Critical, message);

            tc.VerifyMessageAsNotLogged(LogLevel.Critical, message);
        }
    }
}
