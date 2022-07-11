using NClient.Providers.Mapping.Results;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class UseResultsExtensions
    {
        /// <summary>Sets the mapper that converts NClient requests and responses into NClient results with deserialized data.</summary>
        public static INClientResponseMappingSelector<TRequest, TResponse> UseResults<TRequest, TResponse>(
            this INClientResponseMappingSetter<TRequest, TResponse> responseMappingSetter)
        {
            return responseMappingSetter.Use(new ResponseToResultMapperProvider());
        }
    }
}
