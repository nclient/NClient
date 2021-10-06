using System;
using NClient.Abstractions.Ensuring;
using NClient.Abstractions.Exceptions;
using NClient.Abstractions.Resilience;
using RestSharp;

// ReSharper disable once CheckNamespace
namespace NClient.Providers.HttpClient.System
{
    public class DefaultRestSharpEnsuringSettings : IEnsuringSettings<IRestRequest, IRestResponse>
    {
        public Predicate<ResponseContext<IRestRequest, IRestResponse>> IsSuccess { get; }
        public Action<ResponseContext<IRestRequest, IRestResponse>> OnFailure { get; }
        
        public DefaultRestSharpEnsuringSettings() : this(
            isSuccess: responseContext => 
                responseContext.Response.IsSuccessful,
            onFailure: responseContext => 
                throw new HttpClientException<IRestRequest, IRestResponse>(responseContext.Request, responseContext.Response, responseContext.Response.ErrorMessage, responseContext.Response.ErrorException!))
        {
        }
        
        public DefaultRestSharpEnsuringSettings(
            Predicate<ResponseContext<IRestRequest, IRestResponse>> isSuccess, 
            Action<ResponseContext<IRestRequest, IRestResponse>> onFailure)
        {
            IsSuccess = isSuccess;
            OnFailure = onFailure;
        }
    }
}
