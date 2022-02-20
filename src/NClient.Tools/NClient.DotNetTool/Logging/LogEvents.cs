using Microsoft.Extensions.Logging;

namespace NClient.DotNetTool.Logging
{
    public static class LogEvents
    {
        public static readonly EventId Done = new(id: 1000, nameof(Done));
    }
}
