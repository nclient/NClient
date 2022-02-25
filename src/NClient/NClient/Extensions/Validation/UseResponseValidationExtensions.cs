using System.Net.Http;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class UseResponseValidationExtensions
    {
        /// <summary>Sets default response validation of the contents received from HTTP transport.</summary>
        public static INClientResponseValidationSelector<HttpRequestMessage, HttpResponseMessage> Use(
            this INClientTransportResponseValidationSetter<HttpRequestMessage, HttpResponseMessage> transportResponseValidationSetter)
        {
            return transportResponseValidationSetter.UseSystemNetHttpResponseValidation();
        }
    }
}
