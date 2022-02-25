using NClient.Providers.Mapping.LanguageExt;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class UseLanguageExtExtensions
    {
        /// <summary>Sets the mapper that can convert NClient responses into Either monad from LanguageExt.</summary>
        public static INClientResponseMappingSelector<TRequest, TResponse> UseLanguageExtMonads<TRequest, TResponse>(
            this INClientResponseMappingSetter<TRequest, TResponse> optionalBuilder)
        {
            return optionalBuilder.Use(new ResponseToEitherBuilderProvider());
        }
    }
}
