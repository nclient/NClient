using System;
using System.Net.Http;
using NClient.Exceptions;
using NClient.Providers.Transport.SystemNetHttp.Helpers;
using NClient.Providers.Validation;

// ReSharper disable once CheckNamespace
namespace NClient.Providers.Transport.SystemNetHttp
{
    /// <summary>The default response validation settings for System.Net.Http based transport.</summary>
    public class DefaultSystemNetHttpResponseValidatorSettings : IResponseValidatorSettings<HttpRequestMessage, HttpResponseMessage>
    {
        /// <summary>Gets the predicate for determining the success of the response.</summary>
        public Predicate<IResponseContext<HttpRequestMessage, HttpResponseMessage>> IsSuccess { get; }
        
        /// <summary>Gets the action that will be invoked if the response is unsuccessful.</summary>
        public Action<IResponseContext<HttpRequestMessage, HttpResponseMessage>> OnFailure { get; }
        
        /// <summary>Initializes the default response validation settings for System.Net.Http based transport.</summary>
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
        
        /// <summary>Initializes the default response validation settings for System.Net.Http based transport with custom changes.</summary>
        /// <param name="isSuccess">The predicate for determining the success of the response.</param>
        /// <param name="onFailure">The action that will be invoked if the response is unsuccessful.</param>
        public DefaultSystemNetHttpResponseValidatorSettings(
            Predicate<IResponseContext<HttpRequestMessage, HttpResponseMessage>> isSuccess, 
            Action<IResponseContext<HttpRequestMessage, HttpResponseMessage>> onFailure)
        {
            IsSuccess = isSuccess;
            OnFailure = onFailure;
        }
    }
}
