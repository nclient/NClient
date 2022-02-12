using System;
using System.IO;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Console;

namespace NClient.DotNetTool.Logging
{
    public class SimpleConsoleFormatter : ConsoleFormatter
    {
        private const string DefaultForegroundColor = "\x1B[39m\x1B[22m"; // reset to default foreground color

        public SimpleConsoleFormatter() : base(nameof(SimpleConsoleFormatter))
        {
        }
        
        public override void Write<TState>(in LogEntry<TState> logEntry, IExternalScopeProvider scopeProvider, TextWriter textWriter)
        {
            var message = logEntry.Formatter?.Invoke(logEntry.State, logEntry.Exception);
            if (message is null)
                return;

            var consoleColor = GetLogLevelConsoleColors(logEntry.LogLevel);

            if (consoleColor is not null)
                textWriter.WriteLine(GetForegroundColorEscapeCode(consoleColor.Value));
            
            textWriter.WriteLine(message);
            
            if (consoleColor is not null)
                textWriter.WriteLine(DefaultForegroundColor);
        }
        
        private ConsoleColor? GetLogLevelConsoleColors(LogLevel logLevel)
        {
            if (Console.IsOutputRedirected)
                return null;

            return logLevel switch
            {
                LogLevel.Error or LogLevel.Critical => ConsoleColor.Red,
                LogLevel.Warning => ConsoleColor.Yellow,
                LogLevel.None => ConsoleColor.Green,
                _ => null
            };
        }
        
        private static string GetForegroundColorEscapeCode(ConsoleColor color)
        {
            return color switch
            {
                ConsoleColor.Black => "\x1B[30m",
                ConsoleColor.DarkRed => "\x1B[31m",
                ConsoleColor.DarkGreen => "\x1B[32m",
                ConsoleColor.DarkYellow => "\x1B[33m",
                ConsoleColor.DarkBlue => "\x1B[34m",
                ConsoleColor.DarkMagenta => "\x1B[35m",
                ConsoleColor.DarkCyan => "\x1B[36m",
                ConsoleColor.Gray => "\x1B[37m",
                ConsoleColor.Red => "\x1B[1m\x1B[31m",
                ConsoleColor.Green => "\x1B[1m\x1B[32m",
                ConsoleColor.Yellow => "\x1B[1m\x1B[33m",
                ConsoleColor.Blue => "\x1B[1m\x1B[34m",
                ConsoleColor.Magenta => "\x1B[1m\x1B[35m",
                ConsoleColor.Cyan => "\x1B[1m\x1B[36m",
                ConsoleColor.White => "\x1B[1m\x1B[37m",
                _ => DefaultForegroundColor
            };
        }
    }
}
