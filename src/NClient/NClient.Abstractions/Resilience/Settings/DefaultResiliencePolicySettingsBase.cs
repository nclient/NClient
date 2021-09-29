using System;
using System.Threading.Tasks;

namespace NClient.Abstractions.Resilience.Settings
{
    public abstract class DefaultResiliencePolicySettingsBase<TRequest, TResponse> : IResiliencePolicySettings<TRequest, TResponse>
    {
        public virtual int RetryCount { get; set; } = 2;
        public virtual Func<int, TimeSpan> SleepDuration { get; set; } = retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt));
        public abstract Func<ResponseContext<TRequest, TResponse>, bool> ResultPredicate { get; set; }
        public abstract Func<ResponseContext<TRequest, TResponse>, Task> OnFallbackAsync { get; set; }
    }
}
