using System.Net.Http;
using NClient.Abstractions.Building;
using NClient.Providers.HttpClient.System;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class EnsuringSuccessExtensions
    {
        public static INClientOptionalBuilder<TClient, HttpRequestMessage, HttpResponseMessage> EnsuringSuccess<TClient>(
            this INClientOptionalBuilder<TClient, HttpRequestMessage, HttpResponseMessage> factoryOptionalBuilder) 
            where TClient : class
        {
            return factoryOptionalBuilder.EnsuringSystemSuccess();
        }
        
        public static INClientFactoryOptionalBuilder<HttpRequestMessage, HttpResponseMessage> EnsuringSuccess(
            this INClientFactoryOptionalBuilder<HttpRequestMessage, HttpResponseMessage> factoryOptionalBuilder)
        {
            return factoryOptionalBuilder.EnsuringSystemSuccess();
        }
    }
}
