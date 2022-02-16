using System;
using System.Net.Http;
using NClient.Providers.Validation;

// ReSharper disable once CheckNamespace
namespace NClient.Providers.Transport.SystemNetHttp
{
    public class SystemNetHttpResponseValidatorSettings : IResponseValidatorSettings<HttpRequestMessage, HttpResponseMessage>
    {
        public Predicate<IResponseContext<HttpRequestMessage, HttpResponseMessage>> IsSuccess { get; }
        public Action<IResponseContext<HttpRequestMessage, HttpResponseMessage>> OnFailure { get; }
        
        public SystemNetHttpResponseValidatorSettings(
            Predicate<IResponseContext<HttpRequestMessage, HttpResponseMessage>> isSuccess, 
            Action<IResponseContext<HttpRequestMessage, HttpResponseMessage>> onFailure)
        {
            IsSuccess = isSuccess;
            OnFailure = onFailure;
        }
    }
}
