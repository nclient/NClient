using System;
using System.Net.Http;
using NClient.Providers.Resilience;

// ReSharper disable once CheckNamespace
namespace NClient.Providers.HttpClient.System
{
    public class DefaultSystemResiliencePolicySettings : SystemResiliencePolicySettings
    {
        public DefaultSystemResiliencePolicySettings() : this(maxRetries: null, getDelay: null, shouldRetry: null)
        {
        }
        
        public DefaultSystemResiliencePolicySettings(
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
