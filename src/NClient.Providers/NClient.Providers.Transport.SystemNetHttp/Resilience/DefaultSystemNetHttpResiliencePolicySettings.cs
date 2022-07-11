using System;
using System.Net.Http;

// ReSharper disable once CheckNamespace
namespace NClient.Providers.Transport.SystemNetHttp
{
    /// <summary>The default resilience policy settings for System.Net.Http based transport.</summary>
    public class DefaultSystemNetHttpResiliencePolicySettings : SystemNetHttpResiliencePolicySettings
    {
        /// <summary>Creates default resilience policy settings for System.Net.Http based transport.</summary>
        public DefaultSystemNetHttpResiliencePolicySettings() : this(maxRetries: null, getDelay: null, shouldRetry: null)
        {
        }
        
        /// <summary>Creates default resilience policy settings for System.Net.Http based transport with custom changes.</summary>
        /// <param name="maxRetries">The max number of retries.</param>
        /// <param name="getDelay">The function that provides the duration to wait for for a particular retry attempt.</param>
        /// <param name="shouldRetry">The predicate to filter the results this policy will handle.</param>
        public DefaultSystemNetHttpResiliencePolicySettings(
            int? maxRetries = null, 
            Func<int, TimeSpan>? getDelay = null, 
            Func<IResponseContext<HttpRequestMessage, HttpResponseMessage>, bool>? shouldRetry = null) 
            : base(
                maxRetries: maxRetries ?? 2, 
                getDelay: getDelay ?? (retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))), 
                shouldRetry: shouldRetry ?? (responseContext => !responseContext.Response.IsSuccessStatusCode))
        {
        }
    }
}
