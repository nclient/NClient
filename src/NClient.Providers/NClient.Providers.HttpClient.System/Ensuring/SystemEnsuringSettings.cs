using System;
using System.Net.Http;
using NClient.Abstractions.Ensuring;
using NClient.Abstractions.Resilience;

// ReSharper disable once CheckNamespace
namespace NClient.Providers.HttpClient.System
{
    public class SystemEnsuringSettings : EnsuringSettings<HttpRequestMessage, HttpResponseMessage>
    {
        public SystemEnsuringSettings(
            Predicate<ResponseContext<HttpRequestMessage, HttpResponseMessage>> isSuccess, 
            Action<ResponseContext<HttpRequestMessage, HttpResponseMessage>> onFailure) 
            : base(isSuccess, onFailure)
        {
        }
    }
}
