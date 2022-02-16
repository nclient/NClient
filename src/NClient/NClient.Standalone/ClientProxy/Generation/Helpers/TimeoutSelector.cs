using System;
using System.Threading;
using NClient.Standalone.Exceptions.Factories;

namespace NClient.Standalone.ClientProxy.Generation.Helpers
{
    internal interface ITimeoutSelector
    {
        TimeSpan Get(TimeSpan transportTimeout, TimeSpan? clientTimeout, TimeSpan? staticTimeout);
    }
    
    internal class TimeoutSelector : ITimeoutSelector
    {
        private readonly IClientValidationExceptionFactory _clientValidationExceptionFactory;
        
        public TimeoutSelector(IClientValidationExceptionFactory clientValidationExceptionFactory)
        {
            _clientValidationExceptionFactory = clientValidationExceptionFactory;
        }
        
        public TimeSpan Get(TimeSpan transportTimeout, TimeSpan? clientTimeout, TimeSpan? staticTimeout)
        {
            if (transportTimeout != Timeout.InfiniteTimeSpan)
                throw _clientValidationExceptionFactory.TransportTimeoutShouldBeInfinite(transportTimeout);

            return clientTimeout ?? staticTimeout ?? TimeSpan.FromSeconds(100);
        }
    }
}
