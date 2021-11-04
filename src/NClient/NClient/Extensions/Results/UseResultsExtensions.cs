using NClient.Providers.Results;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class UseResultsExtensions
    {
        public static INClientResultsSelector<TRequest, TResponse> UseResults<TRequest, TResponse>(
            this INClientResultsSetter<TRequest, TResponse> resultsSetter)
        {
            return resultsSetter.Use(new ResultBuilderProvider());
        }
    }
}
