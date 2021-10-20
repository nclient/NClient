using System.Net.Http;
using NClient.Abstractions.Building;
using NClient.Providers.HttpClient.System;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class ResponseValidationExtensions
    {
        public static INClientOptionalBuilder<TClient, HttpRequestMessage, HttpResponseMessage> WithSystemResponseValidation<TClient>(
            this INClientOptionalBuilder<TClient, HttpRequestMessage, HttpResponseMessage> factoryOptionalBuilder) 
            where TClient : class
        {
            return factoryOptionalBuilder.WithCustomResponseValidation(new DefaultSystemResponseValidatorSettings());
        }
        
        public static INClientFactoryOptionalBuilder<HttpRequestMessage, HttpResponseMessage> WithSystemResponseValidation(
            this INClientFactoryOptionalBuilder<HttpRequestMessage, HttpResponseMessage> factoryOptionalBuilder)
        {
            return factoryOptionalBuilder.WithCustomResponseValidation(new DefaultSystemResponseValidatorSettings());
        }
    }
}
