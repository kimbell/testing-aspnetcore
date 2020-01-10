using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Testing.AspNetCore.Tests.Infrastructure
{
    /// <summary>
    /// This provider allows us to capture log messages and show them for each xUnit test
    /// </summary>
    internal class TestOutputLoggerProvider : ILoggerProvider
    {
        private readonly ITestOutputHelper _output;
        private readonly ConcurrentDictionary<string, TestOutputLogger> _loggers = new ConcurrentDictionary<string, TestOutputLogger>();

        /// <summary>
        /// All the entries that get logged
        /// </summary>
        public List<LogEntry> Entries { get; } = new List<LogEntry>();

        public TestOutputLoggerProvider(ITestOutputHelper output)
        {
            _output = output;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName, name => new TestOutputLogger(_output, categoryName, Entries));
        }

        public void Dispose()
        {
            Entries.Clear(); 
            _loggers.Clear();
        }
    }

    internal class TestOutputLogger : ILogger
    {
        private readonly ITestOutputHelper _output;
        private readonly string _category;
        private readonly List<LogEntry> _entries;

        public TestOutputLogger(ITestOutputHelper output, string category, List<LogEntry> entries)
        {
            _output = output;
            _category = category;
            _entries = entries;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(Microsoft.Extensions.Logging.LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(Microsoft.Extensions.Logging.LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var formattetMessage = formatter(state, exception);
            _entries.Add(new LogEntry
            {
                Category = _category,
                LogLevel = logLevel,
                Message = formattetMessage
            });

            // show in xUnit logs
            _output?.WriteLine(formattetMessage);
            if (exception != null)
            {
                _output?.WriteLine(exception.StackTrace);
            }
        }
    }

    internal class LogEntry
    {
        public string Category { get; set; }
        public Microsoft.Extensions.Logging.LogLevel LogLevel { get; set; }
        public string Message { get; set; }
    }
}
