using System;
using System.Net.Http;

// ReSharper disable once CheckNamespace
namespace NClient.Providers.Transport.SystemNetHttp
{
    public class DefaultSystemNetHttpResiliencePolicySettings : SystemNetHttpResiliencePolicySettings
    {
        public DefaultSystemNetHttpResiliencePolicySettings() : this(maxRetries: null, getDelay: null, shouldRetry: null)
        {
        }
        
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
