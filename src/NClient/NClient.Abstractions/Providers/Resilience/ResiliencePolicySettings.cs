using System;
using NClient.Providers.Transport;

namespace NClient.Providers.Resilience
{
    /// <summary>The settings for resilience policy provider.</summary>
    /// <typeparam name="TRequest">The type of request that is used in the transport implementation.</typeparam>
    /// <typeparam name="TResponse">The type of response that is used in the transport implementation.</typeparam>
    public class ResiliencePolicySettings<TRequest, TResponse> : IResiliencePolicySettings<TRequest, TResponse>
    {
        /// <summary>The max number of retries.</summary>
        public int MaxRetries { get; }
        
        /// <summary>The function that provides the duration to wait for for a particular retry attempt.</summary>
        public Func<int, TimeSpan> GetDelay { get; }
        
        /// <summary>The predicate to filter the results this policy will handle.</summary>
        public Func<IResponseContext<TRequest, TResponse>, bool> ShouldRetry { get; }
        
        /// <summary>Initializes the settings for resilience policy provider.</summary>
        /// <param name="maxRetries">The max number of retries.</param>
        /// <param name="getDelay">The function that provides the duration to wait for for a particular retry attempt.</param>
        /// <param name="shouldRetry">The predicate to filter the results this policy will handle.</param>
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
