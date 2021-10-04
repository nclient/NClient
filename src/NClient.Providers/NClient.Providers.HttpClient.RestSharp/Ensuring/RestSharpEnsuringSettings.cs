using System;
using NClient.Abstractions.Ensuring;
using NClient.Abstractions.Resilience;
using RestSharp;

// ReSharper disable once CheckNamespace
namespace NClient.Providers.HttpClient.System
{
    public class RestSharpEnsuringSettings : EnsuringSettings<IRestRequest, IRestResponse>
    {
        public RestSharpEnsuringSettings(
            Predicate<ResponseContext<IRestRequest, IRestResponse>> isSuccess, 
            Action<ResponseContext<IRestRequest, IRestResponse>> onFailure) 
            : base(isSuccess, onFailure)
        {
        }
    }
}
