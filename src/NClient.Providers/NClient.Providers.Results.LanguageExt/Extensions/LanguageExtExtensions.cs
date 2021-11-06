using NClient.Providers.Results.LanguageExt;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class LanguageExtExtensions
    {
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> WithLanguageExtResults<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> optionalBuilder) 
            where TClient : class
        {
            return optionalBuilder.WithAdvancedResults(x => x
                .ForClient().Use(new EitherBuilderProvider()));
        }
        
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> WithLanguageExtResults<TRequest, TResponse>(
            this INClientFactoryOptionalBuilder<TRequest, TResponse> optionalBuilder)
        {
            return optionalBuilder.WithAdvancedResults(x => x
                .ForClient().Use(new EitherBuilderProvider()));
        }
    }
}
