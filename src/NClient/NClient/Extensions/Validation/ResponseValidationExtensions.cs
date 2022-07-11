using System.Net.Http;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class ResponseValidationExtensions
    {
        /// <summary>Sets default response validation of the contents received from HTTP transport.</summary>
        public static INClientOptionalBuilder<TClient, HttpRequestMessage, HttpResponseMessage> WithResponseValidation<TClient>(
            this INClientOptionalBuilder<TClient, HttpRequestMessage, HttpResponseMessage> factoryOptionalBuilder) 
            where TClient : class
        {
            return factoryOptionalBuilder.WithSystemNetHttpResponseValidation();
        }
        
        /// <summary>Sets default response validation of the contents received from HTTP transport.</summary>
        public static INClientFactoryOptionalBuilder<HttpRequestMessage, HttpResponseMessage> WithResponseValidation(
            this INClientFactoryOptionalBuilder<HttpRequestMessage, HttpResponseMessage> factoryOptionalBuilder)
        {
            return factoryOptionalBuilder.WithSystemNetHttpResponseValidation();
        }
    }
}
