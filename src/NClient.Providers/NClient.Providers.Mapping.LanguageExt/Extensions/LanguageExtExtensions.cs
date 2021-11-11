using NClient.Providers.Mapping.LanguageExt;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class LanguageExtExtensions
    {
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> WithResponseToMonadMapping<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> optionalBuilder) 
            where TClient : class
        {
            return optionalBuilder.WithAdvancedResponseMapping(x => x
                .ForClient().Use(new ResponseToEitherBuilderProvider()));
        }
        
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> WithResponseToMonadMapping<TRequest, TResponse>(
            this INClientFactoryOptionalBuilder<TRequest, TResponse> optionalBuilder)
        {
            return optionalBuilder.WithAdvancedResponseMapping(x => x
                .ForClient().Use(new ResponseToEitherBuilderProvider()));
        }
    }
}
