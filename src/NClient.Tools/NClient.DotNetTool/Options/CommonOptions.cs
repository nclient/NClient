using CommandLine;
using Microsoft.Extensions.Logging;

namespace NClient.DotNetTool.Options
{
    public abstract class CommonOptions
    {
        protected CommonOptions(LogLevel logLevel)
        {
            LogLevel = logLevel;
        }
        
        [Option(Default = LogLevel.Information)]
        public LogLevel LogLevel { get; }
    }
}
