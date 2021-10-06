using System;
using System.Net.Http;
using NClient.Abstractions.Ensuring;
using NClient.Abstractions.Resilience;

// ReSharper disable once CheckNamespace
namespace NClient.Providers.HttpClient.System
{
    public class SystemEnsuringSettings : IEnsuringSettings<HttpRequestMessage, HttpResponseMessage>
    {
        public Predicate<ResponseContext<HttpRequestMessage, HttpResponseMessage>> IsSuccess { get; }
        public Action<ResponseContext<HttpRequestMessage, HttpResponseMessage>> OnFailure { get; }
        
        public SystemEnsuringSettings(
            Predicate<ResponseContext<HttpRequestMessage, HttpResponseMessage>> isSuccess, 
            Action<ResponseContext<HttpRequestMessage, HttpResponseMessage>> onFailure)
        {
            IsSuccess = isSuccess;
            OnFailure = onFailure;
        }
    }
}
