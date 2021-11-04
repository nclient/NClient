using System.Net.Http;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class ResponseValidationExtensions
    {
        public static INClientOptionalBuilder<TClient, HttpRequestMessage, HttpResponseMessage> WithResponseValidation<TClient>(
            this INClientOptionalBuilder<TClient, HttpRequestMessage, HttpResponseMessage> factoryOptionalBuilder) 
            where TClient : class
        {
            return factoryOptionalBuilder.WithSystemResponseValidation();
        }
        
        public static INClientResponseValidationSelector<HttpRequestMessage, HttpResponseMessage> Use(
            this INClientTransportResponseValidationSetter<HttpRequestMessage, HttpResponseMessage> transportResponseValidationSetter)
        {
            return transportResponseValidationSetter.UseSystemResponseValidation();
        }
        
        public static INClientFactoryOptionalBuilder<HttpRequestMessage, HttpResponseMessage> WithResponseValidation(
            this INClientFactoryOptionalBuilder<HttpRequestMessage, HttpResponseMessage> factoryOptionalBuilder)
        {
            return factoryOptionalBuilder.UseSystemResponseValidation();
        }
    }
}
