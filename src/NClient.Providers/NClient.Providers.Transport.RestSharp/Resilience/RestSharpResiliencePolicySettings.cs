using System;
using NClient.Providers.Resilience;
using RestSharp;

// ReSharper disable once CheckNamespace
namespace NClient.Providers.Transport.RestSharp
{
    /// <summary>The customizable resilience policy settings for RestSharp based transport.</summary>
    public class RestSharpResiliencePolicySettings : IResiliencePolicySettings<IRestRequest, IRestResponse>
    {
        /// <summary>Gets the max number of retries.</summary>
        public int MaxRetries { get; }
        
        /// <summary>Gets the function that provides the duration to wait for for a particular retry attempt.</summary>
        public Func<int, TimeSpan> GetDelay { get; }
        
        /// <summary>Gets the predicate to filter the results this policy will handle.</summary>
        public Func<IResponseContext<IRestRequest, IRestResponse>, bool> ShouldRetry { get; }
        
        /// <summary>Initializes a custom resilience policy settings for RestSharp based transport.</summary>
        /// <param name="maxRetries">The max number of retries.</param>
        /// <param name="getDelay">The function that provides the duration to wait for for a particular retry attempt.</param>
        /// <param name="shouldRetry">The predicate to filter the results this policy will handle.</param>
        public RestSharpResiliencePolicySettings(int maxRetries, Func<int, TimeSpan> getDelay, Func<IResponseContext<IRestRequest, IRestResponse>, bool> shouldRetry)
        {
            MaxRetries = maxRetries;
            GetDelay = getDelay;
            ShouldRetry = shouldRetry;
        }
    }
}
