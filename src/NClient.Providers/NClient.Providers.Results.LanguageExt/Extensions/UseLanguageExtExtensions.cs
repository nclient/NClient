using NClient.Providers.Results.LanguageExt;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class UseHttpResultsExtensions
    {
        public static INClientResultsSelector<TRequest, TResponse> UseLanguageExt<TRequest, TResponse>(
            this INClientResultsSetter<TRequest, TResponse> clientOptionalBuilder)
        {
            return clientOptionalBuilder.Use(new EitherBuilderProvider());
        }
    }
}
