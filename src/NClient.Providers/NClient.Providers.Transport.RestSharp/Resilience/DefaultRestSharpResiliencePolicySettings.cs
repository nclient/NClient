using System;

// ReSharper disable once CheckNamespace
namespace NClient.Providers.Transport.RestSharp
{
    public class DefaultRestSharpResiliencePolicySettings : RestSharpResiliencePolicySettings
    {
        public DefaultRestSharpResiliencePolicySettings() : base(
            maxRetries: 2, 
            getDelay: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), 
            shouldRetry: responseContext => !responseContext.Response.IsSuccessful)
        {
        }
    }
}
