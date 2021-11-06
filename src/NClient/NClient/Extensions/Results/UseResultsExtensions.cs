using NClient.Providers.Mapping.Results;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class UseResultsExtensions
    {
        public static INClientResponseMappingSelector<TRequest, TResponse> UseResults<TRequest, TResponse>(
            this INClientResponseMappingSetter<TRequest, TResponse> responseMappingSetter)
        {
            return responseMappingSetter.Use(new ResponseToResultMapperProvider());
        }
    }
}
