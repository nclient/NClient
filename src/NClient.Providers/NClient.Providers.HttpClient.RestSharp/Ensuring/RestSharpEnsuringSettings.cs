using System;
using NClient.Abstractions.Ensuring;
using NClient.Abstractions.Resilience;
using RestSharp;

// ReSharper disable once CheckNamespace
namespace NClient.Providers.HttpClient.System
{
    public class RestSharpEnsuringSettings : IEnsuringSettings<IRestRequest, IRestResponse>
    {
        public Predicate<IResponseContext<IRestRequest, IRestResponse>> IsSuccess { get; }
        public Action<IResponseContext<IRestRequest, IRestResponse>> OnFailure { get; }
        
        public RestSharpEnsuringSettings(
            Predicate<IResponseContext<IRestRequest, IRestResponse>> isSuccess, 
            Action<IResponseContext<IRestRequest, IRestResponse>> onFailure)
        {
            IsSuccess = isSuccess;
            OnFailure = onFailure;
        }
    }
}
