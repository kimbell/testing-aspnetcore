﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Testing.AspNetCore.Tests.Infrastructure;
using Xunit;
using Xunit.Abstractions;

using MS = Microsoft.Extensions.Logging;

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

            using (var tc = new TestContext(_output))
            {
                tc.AdditionalConfiguration = (context, builder) =>
                {
                    SetMinimumLoggingLevel(builder, LogLevel.Trace);
                };

                await tc.ApiClient.LogMessageAsync(LogLevel.Trace, message);

                tc.VerifyLog(MS.LogLevel.Trace, message, category, count: 1);
            }
        }

        [Fact]
        public async Task TraceMessageNotLogged()
        {
            const string message = "This is a trace message";

            using (var tc = new TestContext(_output))
            {
                tc.AdditionalConfiguration = (context, builder) =>
                {
                    SetMinimumLoggingLevel(builder, LogLevel.None);
                };

                await tc.ApiClient.LogMessageAsync(LogLevel.Trace, message);

                tc.VerifyLog(MS.LogLevel.Trace, message, count: 0);
            }
        }

        [Fact]
        public async Task DebugMessageLogged()
        {
            const string message = "This is a debug message";
            const string category = "Testing.AspNetCore.Controllers.LoggingController";

            using (var tc = new TestContext(_output))
            {
                tc.AdditionalConfiguration = (context, builder) =>
                {
                    SetMinimumLoggingLevel(builder, LogLevel.Debug);
                };

                await tc.ApiClient.LogMessageAsync(LogLevel.Debug, message);

                tc.VerifyLog(MS.LogLevel.Debug, message, category, count: 1);
            }
        }

        [Fact]
        public async Task DebugMessageNotLogged()
        {
            const string message = "This is a debug message";

            using (var tc = new TestContext(_output))
            {
                tc.AdditionalConfiguration = (context, builder) =>
                {
                    SetMinimumLoggingLevel(builder, LogLevel.None);
                };

                await tc.ApiClient.LogMessageAsync(LogLevel.Debug, message);

                tc.VerifyLog(MS.LogLevel.Debug, message, count: 0);
            }
        }

        [Fact]
        public async Task InformationMessageLogged()
        {
            const string message = "This is a information message";
            const string category = "Testing.AspNetCore.Controllers.LoggingController";

            using (var tc = new TestContext(_output))
            {
                tc.AdditionalConfiguration = (context, builder) =>
                {
                    SetMinimumLoggingLevel(builder, LogLevel.Information);
                };

                await tc.ApiClient.LogMessageAsync(LogLevel.Information, message);

                tc.VerifyLog(MS.LogLevel.Information, message, category, count: 1);
            }
        }

        [Fact]
        public async Task InformationMessageNotLogged()
        {
            const string message = "This is a information message";

            using (var tc = new TestContext(_output))
            {
                tc.AdditionalConfiguration = (context, builder) =>
                {
                    SetMinimumLoggingLevel(builder, LogLevel.None);
                };

                await tc.ApiClient.LogMessageAsync(LogLevel.Information, message);

                tc.VerifyLog(MS.LogLevel.Information, message, count: 0);
            }
        }
        
        [Fact]
        public async Task WarningMessageLogged()
        {
            const string message = "This is a warning message";
            const string category = "Testing.AspNetCore.Controllers.LoggingController";

            using (var tc = new TestContext(_output))
            {
                tc.AdditionalConfiguration = (context, builder) =>
                {
                    SetMinimumLoggingLevel(builder, LogLevel.Warning);
                };

                await tc.ApiClient.LogMessageAsync(LogLevel.Warning, message);

                tc.VerifyLog(MS.LogLevel.Warning, message, category, count: 1);
            }
        }

        [Fact]
        public async Task WarningMessageNotLogged()
        {
            const string message = "This is a warning message";

            using (var tc = new TestContext(_output))
            {
                tc.AdditionalConfiguration = (context, builder) =>
                {
                    SetMinimumLoggingLevel(builder, LogLevel.None);
                };

                await tc.ApiClient.LogMessageAsync(LogLevel.Warning, message);
                tc.VerifyLog(MS.LogLevel.Warning, message, count: 0);
            }
        }

        [Fact]
        public async Task ErrorMessageLogged()
        {
            const string message = "This is a error message";
            const string category = "Testing.AspNetCore.Controllers.LoggingController";

            using (var tc = new TestContext(_output))
            {
                tc.AdditionalConfiguration = (context, builder) =>
                {
                    SetMinimumLoggingLevel(builder, LogLevel.Error);
                };

                await tc.ApiClient.LogMessageAsync(LogLevel.Error, message);

                tc.VerifyLog(MS.LogLevel.Error, message, category, count: 1);
            }
        }

        [Fact]
        public async Task ErrorMessageNotLogged()
        {
            const string message = "This is a error message";

            using (var tc = new TestContext(_output))
            {
                tc.AdditionalConfiguration = (context, builder) =>
                {
                    SetMinimumLoggingLevel(builder, LogLevel.None);
                };

                await tc.ApiClient.LogMessageAsync(LogLevel.Error, message);

                tc.VerifyLog(MS.LogLevel.Error, message, count: 0);
            }
        }

        [Fact]
        public async Task CriticalMessageLogged()
        {
            const string message = "This is a critical message";
            const string category = "Testing.AspNetCore.Controllers.LoggingController";

            using (var tc = new TestContext(_output))
            {
                tc.AdditionalConfiguration = (context, builder) =>
                {
                    SetMinimumLoggingLevel(builder, LogLevel.Critical);
                };

                await tc.ApiClient.LogMessageAsync(LogLevel.Critical, message);

                tc.VerifyLog(MS.LogLevel.Critical, message, category, count: 1);
            }
        }

        [Fact]
        public async Task CriticalMessageNotLogged()
        {
            const string message = "This is a critical message";

            using (var tc = new TestContext(_output))
            {
                tc.AdditionalConfiguration = (context, builder) =>
                {
                    SetMinimumLoggingLevel(builder, LogLevel.None);
                };

                await tc.ApiClient.LogMessageAsync(LogLevel.Critical, message);

                tc.VerifyLog(MS.LogLevel.Critical, message, count: 0);
            }
        }

        private static void SetMinimumLoggingLevel(IConfigurationBuilder builder, LogLevel logLevel)
        {
            // since logging is configured very early, we need to use configuration changes to chage it
            builder.AddInMemoryCollection(new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                {"Logging:LogLevel:Default", logLevel.ToString()},
            });
        }
    }
}
