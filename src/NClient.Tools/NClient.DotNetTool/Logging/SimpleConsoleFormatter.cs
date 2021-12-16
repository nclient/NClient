using System;
using System.IO;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Console;

namespace NClient.DotNetTool.Logging
{
    public class SimpleConsoleFormatter : ConsoleFormatter
    {
        public SimpleConsoleFormatter() : base(nameof(SimpleConsoleFormatter))
        {
        }
        
        public override void Write<TState>(in LogEntry<TState> logEntry, IExternalScopeProvider scopeProvider, TextWriter textWriter)
        {
            var message = logEntry.Formatter?.Invoke(logEntry.State, logEntry.Exception);
            if (message is null)
                return;
            
            Console.ForegroundColor = logEntry.LogLevel switch
            {
                LogLevel.Error or LogLevel.Critical => ConsoleColor.Red,
                LogLevel.Warning => ConsoleColor.Yellow,
                LogLevel.None => ConsoleColor.Green,
                _ => ConsoleColor.White
            };

            textWriter.WriteLine(message);
        }
    }
}
