using System;
using NClient.Providers.Resilience;
using RestSharp;

// ReSharper disable once CheckNamespace
namespace NClient.Providers.HttpClient.System
{
    public class RestSharpResiliencePolicySettings : IResiliencePolicySettings<IRestRequest, IRestResponse>
    {
        public int MaxRetries { get; }
        public Func<int, TimeSpan> GetDelay { get; }
        public Func<IResponseContext<IRestRequest, IRestResponse>, bool> ShouldRetry { get; }
        
        public RestSharpResiliencePolicySettings(int maxRetries, Func<int, TimeSpan> getDelay, Func<IResponseContext<IRestRequest, IRestResponse>, bool> shouldRetry)
        {
            MaxRetries = maxRetries;
            GetDelay = getDelay;
            ShouldRetry = shouldRetry;
        }
    }
}
