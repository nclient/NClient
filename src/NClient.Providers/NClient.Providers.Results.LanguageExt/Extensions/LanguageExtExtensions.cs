using NClient.Providers.Results.LanguageExt;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class LanguageExtExtensions
    {
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> WithLanguageExtResults<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> clientOptionalBuilder) 
            where TClient : class
        {
            return clientOptionalBuilder.WithAdvancedResults(x => x
                .ForClient().Use(new EitherBuilderProvider()));
        }
        
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> WithLanguageExtResults<TRequest, TResponse>(
            this INClientFactoryOptionalBuilder<TRequest, TResponse> clientOptionalBuilder)
        {
            return clientOptionalBuilder.WithAdvancedResults(x => x
                .ForClient().Use(new EitherBuilderProvider()));
        }
    }
}
