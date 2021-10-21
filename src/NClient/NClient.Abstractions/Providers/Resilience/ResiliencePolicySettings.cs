using System;

namespace NClient.Abstractions.Providers.Resilience
{
    public class ResiliencePolicySettings<TRequest, TResponse> : IResiliencePolicySettings<TRequest, TResponse>
    {
        public int MaxRetries { get; }
        public Func<int, TimeSpan> GetDelay { get; }
        public Func<IResponseContext<TRequest, TResponse>, bool> ShouldRetry { get; }
        
        public ResiliencePolicySettings(
            int maxRetries, 
            Func<int, TimeSpan> getDelay, 
            Func<IResponseContext<TRequest, TResponse>, bool> shouldRetry)
        {
            MaxRetries = maxRetries;
            GetDelay = getDelay;
            ShouldRetry = shouldRetry;
        }
    }
}
