using Microsoft.Extensions.Logging;

namespace NClient.DotNetTool.Logging
{
    public static class LoggerExtensions
    {
        public static void LogDone(this ILogger logger, string message, params object[] args)
        {
            // ReSharper disable once TemplateIsNotCompileTimeConstantProblem
            logger.LogInformation(LogEvents.Done, message, args);
        }
    }
}
