using NClient.Providers.Mapping;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class StreamsExtensions
    {
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> WithResponseToStreamMapping<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> optionalBuilder) 
            where TClient : class
        {
            return optionalBuilder.WithAdvancedResponseMapping(x => x
                .ForClient().Use(new ResponseToStreamMapperProvider()));
        }
        
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> WithResponseToStreamMapping<TRequest, TResponse>(
            this INClientFactoryOptionalBuilder<TRequest, TResponse> optionalBuilder)
        {
            return optionalBuilder.WithAdvancedResponseMapping(x => x
                .ForClient().Use(new ResponseToStreamMapperProvider()));
        }
    }
}
