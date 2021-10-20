using System.Net.Http;
using NClient.Abstractions.Building;

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
        
        public static INClientFactoryOptionalBuilder<HttpRequestMessage, HttpResponseMessage> WithResponseValidation(
            this INClientFactoryOptionalBuilder<HttpRequestMessage, HttpResponseMessage> factoryOptionalBuilder)
        {
            return factoryOptionalBuilder.WithSystemResponseValidation();
        }
    }
}
