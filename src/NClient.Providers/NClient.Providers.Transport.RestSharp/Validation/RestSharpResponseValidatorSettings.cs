using System;
using NClient.Providers.Validation;
using RestSharp;

// ReSharper disable once CheckNamespace
namespace NClient.Providers.Transport.RestSharp
{
    /// <summary>The customizable response validation settings for RestSharp based transport.</summary>
    public class RestSharpResponseValidatorSettings : IResponseValidatorSettings<IRestRequest, IRestResponse>
    {
        /// <summary>Gets the predicate for determining the success of the response.</summary>
        public Predicate<IResponseContext<IRestRequest, IRestResponse>> IsSuccess { get; }
        
        /// <summary>Gets the action that will be invoked if the response is unsuccessful.</summary>
        public Action<IResponseContext<IRestRequest, IRestResponse>> OnFailure { get; }
        
        /// <summary>Initializes a custom response validation settings for RestSharp based transport.</summary>
        /// <param name="isSuccess">The predicate for determining the success of the response</param>
        /// <param name="onFailure">The action that will be invoked if the response is unsuccessful.</param>
        public RestSharpResponseValidatorSettings(
            Predicate<IResponseContext<IRestRequest, IRestResponse>> isSuccess, 
            Action<IResponseContext<IRestRequest, IRestResponse>> onFailure)
        {
            IsSuccess = isSuccess;
            OnFailure = onFailure;
        }
    }
}
