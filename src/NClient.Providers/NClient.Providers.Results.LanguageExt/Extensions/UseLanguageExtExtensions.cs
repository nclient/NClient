using NClient.Providers.Results.LanguageExt;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class UseHttpResultsExtensions
    {
        public static INClientResultsSelector<TRequest, TResponse> UseLanguageExt<TRequest, TResponse>(
            this INClientResultsSetter<TRequest, TResponse> optionalBuilder)
        {
            return optionalBuilder.Use(new EitherBuilderProvider());
        }
    }
}
