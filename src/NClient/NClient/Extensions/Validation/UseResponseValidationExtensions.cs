using System.Net.Http;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class UseResponseValidationExtensions
    {
        public static INClientResponseValidationSelector<HttpRequestMessage, HttpResponseMessage> Use(
            this INClientTransportResponseValidationSetter<HttpRequestMessage, HttpResponseMessage> transportResponseValidationSetter)
        {
            return transportResponseValidationSetter.UseSystemNetHttpResponseValidation();
        }
    }
}
