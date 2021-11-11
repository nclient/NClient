using NClient.Providers.Mapping.LanguageExt;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class UseLanguageExtExtensions
    {
        public static INClientResponseMappingSelector<TRequest, TResponse> UseLanguageExtMonads<TRequest, TResponse>(
            this INClientResponseMappingSetter<TRequest, TResponse> optionalBuilder)
        {
            return optionalBuilder.Use(new ResponseToEitherBuilderProvider());
        }
    }
}
