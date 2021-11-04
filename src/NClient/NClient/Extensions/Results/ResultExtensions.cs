using NClient.Providers.Results;

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
                .WithResults(x => x
                    .ForClient().Use(new ResultBuilderProvider()))
                .AsBasic();
        }
        
        public static INClientResultsSelector<TRequest, TResponse> UseResults<TRequest, TResponse>(
            this INClientResultsSetter<TRequest, TResponse> resultsSetter)
        {
            return resultsSetter.Use(new ResultBuilderProvider());
        }
        
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> WithResults<TRequest, TResponse>(
            this INClientFactoryOptionalBuilder<TRequest, TResponse> factoryOptionalBuilder)
        {
            return factoryOptionalBuilder.WithCustomResults(new ResultBuilderProvider());
        }
    }
}
