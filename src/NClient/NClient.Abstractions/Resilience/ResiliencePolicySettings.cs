using System;

namespace NClient.Abstractions.Resilience
{
    public class ResiliencePolicySettings<TRequest, TResponse> : IResiliencePolicySettings<TRequest, TResponse>
    {
        public int MaxRetries { get; }
        public Func<int, TimeSpan> GetDelay { get; }
        public Func<IResponseContext<TRequest, TResponse>, bool> ShouldRetry { get; }
        
        public ResiliencePolicySettings(
            int retryCount, 
            Func<int, TimeSpan> sleepDuration, 
            Func<IResponseContext<TRequest, TResponse>, bool> resultPredicate)
        {
            MaxRetries = retryCount;
            GetDelay = sleepDuration;
            ShouldRetry = resultPredicate;
        }
    }
}
