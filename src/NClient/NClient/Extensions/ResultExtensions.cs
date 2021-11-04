using System.Net.Http;
using NClient.Providers.Results;
using NClient.Providers.Results.HttpResults;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class ResultExtensions
    {
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> WithResults<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> clientOptionalBuilder) 
            where TClient : class
        {
            return clientOptionalBuilder.AsAdvanced()
                .WithCustomResults(new ResultBuilderProvider())
                .AsBasic();
        }
        
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> WithResults<TRequest, TResponse>(
            this INClientFactoryOptionalBuilder<TRequest, TResponse> factoryOptionalBuilder)
        {
            return factoryOptionalBuilder.WithCustomResults(new ResultBuilderProvider());
        }
        
        public static INClientOptionalBuilder<TClient, HttpRequestMessage, HttpResponseMessage> WithHttpResults<TClient>(
            this INClientOptionalBuilder<TClient, HttpRequestMessage, HttpResponseMessage> clientOptionalBuilder) 
            where TClient : class
        {
            return clientOptionalBuilder.AsAdvanced()
                .WithCustomResults(new HttpResponseBuilderProvider())
                .AsBasic();
        }
        
        public static INClientFactoryOptionalBuilder<HttpRequestMessage, HttpResponseMessage> WithHttpResults(
            this INClientFactoryOptionalBuilder<HttpRequestMessage, HttpResponseMessage> factoryOptionalBuilder)
        {
            return factoryOptionalBuilder.WithCustomResults(new HttpResponseBuilderProvider());
        }
    }
}
