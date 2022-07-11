using NClient.Providers.Transport.RestSharp;
using RestSharp;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class UseRestSharpResponseValidationExtensions
    {
        /// <summary>Sets default System.Net.Http validation the contents of the response received from transport.</summary>
        public static INClientResponseValidationSelector<IRestRequest, IRestResponse> UseRestSharpResponseValidation(
            this INClientTransportResponseValidationSetter<IRestRequest, IRestResponse> transportResponseValidationSetter)
        {
            return transportResponseValidationSetter.Use(new DefaultRestSharpResponseValidatorSettings());
        }
    }
}
