using NClient.Providers.Results;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class ResultsExtensions
    {
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> WithResults<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> optionalBuilder) 
            where TClient : class
        {
            return optionalBuilder.WithAdvancedResults(x => x
                .ForClient().Use(new ResultBuilderProvider()));
        }
        
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> WithResults<TRequest, TResponse>(
            this INClientFactoryOptionalBuilder<TRequest, TResponse> optionalBuilder)
        {
            return optionalBuilder.WithAdvancedResults(x => x
                .ForClient().Use(new ResultBuilderProvider()));
        }
    }
}
