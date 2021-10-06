using System;

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
        int MaxRetries { get; }
        
        /// <summary>
        /// The function that provides the duration to wait for for a particular retry attempt.
        /// </summary>
        Func<int, TimeSpan> GetDelay { get; }
        
        /// <summary>
        /// The predicate to filter the results this policy will handle.
        /// </summary>
        Func<ResponseContext<TRequest, TResponse>, bool> ShouldRetry { get; }
    }
}
