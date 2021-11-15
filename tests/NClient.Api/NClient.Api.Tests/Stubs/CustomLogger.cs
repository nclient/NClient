using System;
using Microsoft.Extensions.Logging;
using NClient.Standalone.Client.Logging;

namespace NClient.Api.Tests.Stubs
{
    public class CustomLogger : ILogger
    {
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception, string> formatter)
        {
            Console.WriteLine(state?.ToString());
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return new DisposableDecorator(Array.Empty<IDisposable>());
        }
    }
}
