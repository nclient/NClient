using NClient.Abstractions.Ensuring;
using NClient.Abstractions.Exceptions;
using RestSharp;

// ReSharper disable once CheckNamespace
namespace NClient.Providers.HttpClient.System
{
    public class DefaultEnsuringSettings : EnsuringSettings<IRestRequest, IRestResponse>
    {
        public DefaultEnsuringSettings() : base(
            successCondition: x => 
                x.Response.IsSuccessful,
            onFailure: x => 
                throw new HttpClientException<IRestRequest, IRestResponse>(x.Request, x.Response, x.Response.ErrorMessage, x.Response.ErrorException!))
        {
        }
    }
}
