using System;

namespace NClient.Abstractions.Resilience.Settings
{
    public class ResiliencePolicySettings<TRequest, TResponse> : IResiliencePolicySettings<TRequest, TResponse>
    {
        public int RetryCount { get; }
        public Func<int, TimeSpan> SleepDuration { get; }
        public Func<ResponseContext<TRequest, TResponse>, bool> ResultPredicate { get; }
        
        public ResiliencePolicySettings(
            int retryCount, 
            Func<int, TimeSpan> sleepDuration, 
            Func<ResponseContext<TRequest, TResponse>, bool> resultPredicate)
        {
            RetryCount = retryCount;
            SleepDuration = sleepDuration;
            ResultPredicate = resultPredicate;
        }
    }
}
