using System;
using System.Net.Http;
using NClient.Abstractions.Exceptions;
using NClient.Abstractions.Resilience;
using NClient.Abstractions.Validation;

// ReSharper disable once CheckNamespace
namespace NClient.Providers.HttpClient.System
{
    public class DefaultSystemResponseValidatorSettings : IResponseValidatorSettings<HttpRequestMessage, HttpResponseMessage>
    {
        public Predicate<IResponseContext<HttpRequestMessage, HttpResponseMessage>> IsSuccess { get; }
        public Action<IResponseContext<HttpRequestMessage, HttpResponseMessage>> OnFailure { get; }
        
        public DefaultSystemResponseValidatorSettings() : this(
            isSuccess: x => x.Response.IsSuccessStatusCode,
            onFailure: x =>
            {
                try
                {
                    x.Response.EnsureSuccessStatusCode();
                }
                catch (HttpRequestException e)
                {
                    throw new HttpClientException<HttpRequestMessage, HttpResponseMessage>(x.Request, x.Response, e.Message, e);
                }
            })
        {
        }
        
        public DefaultSystemResponseValidatorSettings(
            Predicate<IResponseContext<HttpRequestMessage, HttpResponseMessage>> isSuccess, 
            Action<IResponseContext<HttpRequestMessage, HttpResponseMessage>> onFailure)
        {
            IsSuccess = isSuccess;
            OnFailure = onFailure;
        }
    }
}
