using System;
using NClient.Exceptions;
using NClient.Providers.Validation;
using RestSharp;

// ReSharper disable once CheckNamespace
namespace NClient.Providers.Transport.RestSharp
{
    /// <summary>The default response validation settings for RestSharp based transport.</summary>
    public class DefaultRestSharpResponseValidatorSettings : IResponseValidatorSettings<IRestRequest, IRestResponse>
    {
        /// <summary>Gets the predicate for determining the success of the response.</summary>
        public Predicate<IResponseContext<IRestRequest, IRestResponse>> IsSuccess { get; }
        
        /// <summary>Gets the action that will be invoked if the response is unsuccessful.</summary>
        public Action<IResponseContext<IRestRequest, IRestResponse>> OnFailure { get; }
        
        /// <summary>Initializes the default response validation settings for RestSharp based transport.</summary>
        public DefaultRestSharpResponseValidatorSettings() : this(
            isSuccess: responseContext => 
                responseContext.Response.IsSuccessful,
            onFailure: responseContext => 
                throw new TransportException<IRestRequest, IRestResponse>(responseContext.Request, responseContext.Response, responseContext.Response.ErrorMessage, responseContext.Response.ErrorException!))
        {
        }
        
        /// <summary>Initializes the default response validation settings for RestSharp based transport with custom changes.</summary>
        /// <param name="isSuccess">The predicate for determining the success of the response.</param>
        /// <param name="onFailure">The action that will be invoked if the response is unsuccessful.</param>
        public DefaultRestSharpResponseValidatorSettings(
            Predicate<IResponseContext<IRestRequest, IRestResponse>> isSuccess, 
            Action<IResponseContext<IRestRequest, IRestResponse>> onFailure)
        {
            IsSuccess = isSuccess;
            OnFailure = onFailure;
        }
    }
}
