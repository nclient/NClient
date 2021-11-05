using NClient.Providers.Results;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class ResultsExtensions
    {
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> WithResults<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> clientOptionalBuilder) 
            where TClient : class
        {
            return clientOptionalBuilder.WithAdvancedResults(x => x
                .ForClient().Use(new ResultBuilderProvider()));
        }
        
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> WithResults<TRequest, TResponse>(
            this INClientFactoryOptionalBuilder<TRequest, TResponse> clientOptionalBuilder)
        {
            return clientOptionalBuilder.AsAdvanced()
                .WithResults(x => x
                    .ForClient().Use(new ResultBuilderProvider()))
                .AsBasic();
        }
    }
}
