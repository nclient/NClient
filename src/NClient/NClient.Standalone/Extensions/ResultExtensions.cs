using NClient.Abstractions.Building;
using NClient.Abstractions.Providers.Results;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class ResultExtensions
    {
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> WithResults<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> clientOptionalBuilder) 
            where TClient : class
        {
            return clientOptionalBuilder.WithCustomResults(new ResultBuilderProvider());
        }
        
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> WithResults<TRequest, TResponse>(
            this INClientFactoryOptionalBuilder<TRequest, TResponse> factoryOptionalBuilder)
        {
            return factoryOptionalBuilder.WithCustomResults(new ResultBuilderProvider());
        }
    }
}
