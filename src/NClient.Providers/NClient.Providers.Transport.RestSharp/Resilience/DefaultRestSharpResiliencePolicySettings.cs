using System;
using RestSharp;

// ReSharper disable once CheckNamespace
namespace NClient.Providers.Transport.RestSharp
{
    /// <summary>The default resilience policy settings for RestSharp based transport.</summary>
    public class DefaultRestSharpResiliencePolicySettings : RestSharpResiliencePolicySettings
    {
        /// <summary>Initializes default resilience policy settings for RestSharp based transport.</summary>
        public DefaultRestSharpResiliencePolicySettings() : base(
            maxRetries: 2, 
            getDelay: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), 
            shouldRetry: responseContext => !responseContext.Response.IsSuccessful)
        {
        }
        
        /// <summary>Initializes default resilience policy settings for RestSharp based transport with custom changes.</summary>
        /// <param name="maxRetries">The max number of retries.</param>
        /// <param name="getDelay">The function that provides the duration to wait for for a particular retry attempt.</param>
        /// <param name="shouldRetry">The predicate to filter the results this policy will handle.</param>
        public DefaultRestSharpResiliencePolicySettings(
            int? maxRetries = null, 
            Func<int, TimeSpan>? getDelay = null, 
            Func<IResponseContext<IRestRequest, IRestResponse>, bool>? shouldRetry = null) 
            : base(
                maxRetries: maxRetries ?? 2, 
                getDelay: getDelay ?? (retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))), 
                shouldRetry: shouldRetry ?? (responseContext => !responseContext.Response.IsSuccessful))
        {
        }
    }
}
