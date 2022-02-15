using System;
using System.Net.Http;
using NClient.Exceptions;
using NClient.Providers.Transport.SystemNetHttp.Helpers;
using NClient.Providers.Validation;

// ReSharper disable once CheckNamespace
namespace NClient.Providers.Transport.SystemNetHttp
{
    public class DefaultSystemNetHttpResponseValidatorSettings : IResponseValidatorSettings<HttpRequestMessage, HttpResponseMessage>
    {
        public Predicate<IResponseContext<HttpRequestMessage, HttpResponseMessage>> IsSuccess { get; }
        public Action<IResponseContext<HttpRequestMessage, HttpResponseMessage>> OnFailure { get; }
        
        public DefaultSystemNetHttpResponseValidatorSettings() : this(
            isSuccess: x => x.Response.IsSuccessStatusCode,
            onFailure: x =>
            {
                var httpRequestException = x.Response.TryGetException();
                if (httpRequestException is not null)
                    throw new TransportException<HttpRequestMessage, HttpResponseMessage>(x.Request, x.Response, httpRequestException.Message, httpRequestException);
            })
        {
        }
        
        public DefaultSystemNetHttpResponseValidatorSettings(
            Predicate<IResponseContext<HttpRequestMessage, HttpResponseMessage>> isSuccess, 
            Action<IResponseContext<HttpRequestMessage, HttpResponseMessage>> onFailure)
        {
            IsSuccess = isSuccess;
            OnFailure = onFailure;
        }
    }
}
