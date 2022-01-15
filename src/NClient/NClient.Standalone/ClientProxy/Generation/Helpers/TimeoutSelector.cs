using System;
using System.Threading;

namespace NClient.Standalone.ClientProxy.Generation.Helpers
{
    internal interface ITimeoutSelector
    {
        TimeSpan Get(TimeSpan transportTimeout, TimeSpan? clientTimeout, TimeSpan? staticTimeout);
    }
    
    internal class TimeoutSelector : ITimeoutSelector
    {
        public TimeSpan Get(TimeSpan transportTimeout, TimeSpan? clientTimeout, TimeSpan? staticTimeout)
        {
            if (transportTimeout != Timeout.InfiniteTimeSpan && clientTimeout.HasValue && clientTimeout >= transportTimeout)
                throw new InvalidOperationException();

            if (transportTimeout != Timeout.InfiniteTimeSpan && staticTimeout.HasValue && staticTimeout >= transportTimeout)
                throw new InvalidOperationException();

            return clientTimeout ?? staticTimeout ?? transportTimeout;
        }
    }
}
