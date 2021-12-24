using Microsoft.Extensions.Logging;

namespace NClient.DotNetTool.Logging
{
    public static class LoggerExtensions
    {
        public static void LogDone(this ILogger logger, string message, params object[] args)
        {
            logger.Log(LogLevel.None, message, args);
        }
    }
}
