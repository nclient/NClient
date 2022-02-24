using System;
using System.Net.Http;
using NClient.Providers.Validation;

// ReSharper disable once CheckNamespace
namespace NClient.Providers.Transport.SystemNetHttp
{
    /// <summary>The customizable response validation settings for System.Net.Http based transport.</summary>
    public class SystemNetHttpResponseValidatorSettings : IResponseValidatorSettings<HttpRequestMessage, HttpResponseMessage>
    {
        /// <summary>Gets the predicate for determining the success of the response.</summary>
        public Predicate<IResponseContext<HttpRequestMessage, HttpResponseMessage>> IsSuccess { get; }
        
        /// <summary>Gets the action that will be invoked if the response is unsuccessful.</summary>
        public Action<IResponseContext<HttpRequestMessage, HttpResponseMessage>> OnFailure { get; }
        
        /// <summary>Initializes a custom response validation settings for System.Net.Http based transport.</summary>
        /// <param name="isSuccess">The predicate for determining the success of the response</param>
        /// <param name="onFailure">The action that will be invoked if the response is unsuccessful.</param>
        public SystemNetHttpResponseValidatorSettings(
            Predicate<IResponseContext<HttpRequestMessage, HttpResponseMessage>> isSuccess, 
            Action<IResponseContext<HttpRequestMessage, HttpResponseMessage>> onFailure)
        {
            IsSuccess = isSuccess;
            OnFailure = onFailure;
        }
    }
}
