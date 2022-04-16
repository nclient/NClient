using NClient.Providers.Mapping;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class UseStreamsExtensions
    {
        public static INClientResponseMappingSelector<TRequest, TResponse> UseStreams<TRequest, TResponse>(
            this INClientResponseMappingSetter<TRequest, TResponse> responseMappingSetter)
        {
            return responseMappingSetter.Use(new ResponseToStreamMapperProvider());
        }
    }
}
