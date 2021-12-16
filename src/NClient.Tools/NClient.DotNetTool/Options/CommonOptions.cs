using CommandLine;
using Microsoft.Extensions.Logging;

namespace NClient.DotNetTool.Options
{
    public class CommonOptions
    {
        public CommonOptions(LogLevel logLevel)
        {
            LogLevel = logLevel;
        }
        
        [Option(Default = LogLevel.Information)]
        public LogLevel LogLevel { get; }
    }
}
