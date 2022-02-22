using NClient.Providers.Mapping.Results;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class ResultsExtensions
    {
        /// <summary>Sets the mapper that converts NClient requests and responses into NClient results with deserialized data.</summary>
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> WithResponseToResultMapping<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> optionalBuilder) 
            where TClient : class
        {
            return optionalBuilder.WithAdvancedResponseMapping(x => x
                .ForClient().Use(new ResponseToResultMapperProvider()));
        }
        
        /// <summary>Sets the mapper that converts NClient requests and responses into NClient results with deserialized data.</summary>
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> WithResponseToResultMapping<TRequest, TResponse>(
            this INClientFactoryOptionalBuilder<TRequest, TResponse> optionalBuilder)
        {
            return optionalBuilder.WithAdvancedResponseMapping(x => x
                .ForClient().Use(new ResponseToResultMapperProvider()));
        }
    }
}
