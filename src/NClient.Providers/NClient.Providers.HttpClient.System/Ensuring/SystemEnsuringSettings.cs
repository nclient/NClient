using System;
using System.Net.Http;
using NClient.Abstractions.Ensuring;
using NClient.Abstractions.Resilience;

// ReSharper disable once CheckNamespace
namespace NClient.Providers.HttpClient.System
{
    public class SystemEnsuringSettings : IEnsuringSettings<HttpRequestMessage, HttpResponseMessage>
    {
        public Predicate<IResponseContext<HttpRequestMessage, HttpResponseMessage>> IsSuccess { get; }
        public Action<IResponseContext<HttpRequestMessage, HttpResponseMessage>> OnFailure { get; }
        
        public SystemEnsuringSettings(
            Predicate<IResponseContext<HttpRequestMessage, HttpResponseMessage>> isSuccess, 
            Action<IResponseContext<HttpRequestMessage, HttpResponseMessage>> onFailure)
        {
            IsSuccess = isSuccess;
            OnFailure = onFailure;
        }
    }
}
