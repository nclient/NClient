using System.Linq;
using NClient.Providers.Results;
using NClient.Providers.Transport;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class ExtraResultsExtensions
    {
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> WithResults<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> optionalBuilder,
            IResultBuilder<TRequest, TResponse> builder, params IResultBuilder<TRequest, TResponse>[] extraBuilders) 
            where TClient : class
        {
            return optionalBuilder.WithResults(extraBuilders.Concat(new[] { builder }));
        }

        public static INClientResultsSelector<TRequest, TResponse> Use<TRequest, TResponse>(
            this INClientResultsSetter<TRequest, TResponse> resultsSetter,
            IResultBuilder<IRequest, IResponse> builder, params IResultBuilder<IRequest, IResponse>[] extraBuilders)
        {
            return resultsSetter.Use(extraBuilders.Concat(new[] { builder }));
        }

        public static INClientResultsSelector<TRequest, TResponse> Use<TRequest, TResponse>(
            this INClientResultsSetter<TRequest, TResponse> resultsSetter,
            IResultBuilderProvider<IRequest, IResponse> provider, params IResultBuilderProvider<IRequest, IResponse>[] extraProviders)
        {
            return resultsSetter.Use(extraProviders.Concat(new[] { provider }));
        }

        public static INClientResultsSelector<TRequest, TResponse> Use<TRequest, TResponse>(
            this INClientTransportResultsSetter<TRequest, TResponse> transportResultsSetter,
            IResultBuilder<TRequest, TResponse> builder, params IResultBuilder<TRequest, TResponse>[] extraBuilders)
        {
            return transportResultsSetter.Use(extraBuilders.Concat(new[] { builder }));
        }

        public static INClientResultsSelector<TRequest, TResponse> Use<TRequest, TResponse>(
            this INClientTransportResultsSetter<TRequest, TResponse> transportResultsSetter,
            IResultBuilderProvider<TRequest, TResponse> provider, params IResultBuilderProvider<TRequest, TResponse>[] extraProviders)
        {
            return transportResultsSetter.Use(extraProviders.Concat(new[] { provider }));
        }
    }
}
