using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace NClient.Standalone.Client.Logging
{
    internal class LoggerDecorator<T> : ILogger<T>
    {
        private readonly IEnumerable<ILogger> _loggers;
        
        public LoggerDecorator(IEnumerable<ILogger> loggers)
        {
            _loggers = loggers;
        }
        
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            foreach (var logger in _loggers)
            {
                logger.Log(logLevel, eventId, state, exception, formatter);
            }
        }
        
        public bool IsEnabled(LogLevel logLevel)
        {
            return _loggers.Any(x => x.IsEnabled(logLevel));
        }
        
        public IDisposable BeginScope<TState>(TState state)
        {
            var disposables = new List<IDisposable>();
            try
            {
                disposables.AddRange(_loggers.Select(logger => logger.BeginScope(state)));
                return new DisposableDecorator(disposables);
            }
            catch
            {
                foreach (var disposable in disposables)
                {
                    disposable.Dispose();
                }
                throw;
            }
        }
    }
}
