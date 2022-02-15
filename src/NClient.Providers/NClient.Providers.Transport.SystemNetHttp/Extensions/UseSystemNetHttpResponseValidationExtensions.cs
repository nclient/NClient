using System.Net.Http;
using NClient.Providers.Transport.SystemNetHttp;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class UseSystemNetHttpResponseValidationExtensions
    {
        public static INClientResponseValidationSelector<HttpRequestMessage, HttpResponseMessage> UseSystemNetHttpResponseValidation(
            this INClientTransportResponseValidationSetter<HttpRequestMessage, HttpResponseMessage> transportResponseValidationSetter)
        {
            return transportResponseValidationSetter.Use(new DefaultSystemNetHttpResponseValidatorSettings());
        }
    }
}
