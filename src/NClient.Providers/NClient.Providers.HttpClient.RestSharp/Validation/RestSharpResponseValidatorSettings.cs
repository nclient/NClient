using System;
using NClient.Abstractions.Providers.Resilience;
using NClient.Abstractions.Providers.Validation;
using RestSharp;

// ReSharper disable once CheckNamespace
namespace NClient.Providers.HttpClient.System
{
    public class RestSharpResponseValidatorSettings : IResponseValidatorSettings<IRestRequest, IRestResponse>
    {
        public Predicate<IResponseContext<IRestRequest, IRestResponse>> IsSuccess { get; }
        public Action<IResponseContext<IRestRequest, IRestResponse>> OnFailure { get; }
        
        public RestSharpResponseValidatorSettings(
            Predicate<IResponseContext<IRestRequest, IRestResponse>> isSuccess, 
            Action<IResponseContext<IRestRequest, IRestResponse>> onFailure)
        {
            IsSuccess = isSuccess;
            OnFailure = onFailure;
        }
    }
}
