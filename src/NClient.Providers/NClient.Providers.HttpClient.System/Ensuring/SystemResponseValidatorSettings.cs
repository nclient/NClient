using System;
using System.Net.Http;
using NClient.Abstractions.Resilience;
using NClient.Abstractions.Validation;

// ReSharper disable once CheckNamespace
namespace NClient.Providers.HttpClient.System
{
    public class SystemResponseValidatorSettings : IResponseValidatorSettings<HttpRequestMessage, HttpResponseMessage>
    {
        public Predicate<IResponseContext<HttpRequestMessage, HttpResponseMessage>> IsSuccess { get; }
        public Action<IResponseContext<HttpRequestMessage, HttpResponseMessage>> OnFailure { get; }
        
        public SystemResponseValidatorSettings(
            Predicate<IResponseContext<HttpRequestMessage, HttpResponseMessage>> isSuccess, 
            Action<IResponseContext<HttpRequestMessage, HttpResponseMessage>> onFailure)
        {
            IsSuccess = isSuccess;
            OnFailure = onFailure;
        }
    }
}
