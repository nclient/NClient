using System;
using System.Threading.Tasks;

namespace NClient.Abstractions.Resilience
{
    //TODO: doc
    /// <summary>
    /// 
    /// </summary>
    public interface IResiliencePolicySettings<TRequest, TResponse>
    {
        /// <summary>
        /// The retry count.
        /// </summary>
        int RetryCount { get; }
        
        /// <summary>
        /// The function that provides the duration to wait for for a particular retry attempt.
        /// </summary>
        Func<int, TimeSpan> SleepDuration { get; }
        
        /// <summary>
        /// The predicate to filter the results this policy will handle.
        /// </summary>
        Func<ResponseContext<TRequest, TResponse>, bool> ResultPredicate { get; }
        
        /// <summary>
        /// The function to call asynchronously before invoking the fallback delegate.
        /// </summary>
        Func<ResponseContext<TRequest, TResponse>, Task> OnFallbackAsync { get; }
    }
}
