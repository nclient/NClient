using System.Linq;
using NClient.Providers.Mapping;
using NClient.Providers.Transport;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class ExtraResultsExtensions
    {
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> WithResults<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> optionalBuilder,
            IResponseMapper<TRequest, TResponse> builder, params IResponseMapper<TRequest, TResponse>[] extraBuilders) 
            where TClient : class
        {
            return optionalBuilder.WithResponseMapping(extraBuilders.Concat(new[] { builder }));
        }

        public static INClientResponseMappingSelector<TRequest, TResponse> Use<TRequest, TResponse>(
            this INClientResponseMappingSetter<TRequest, TResponse> responseMappingSetter,
            IResponseMapper<IRequest, IResponse> builder, params IResponseMapper<IRequest, IResponse>[] extraBuilders)
        {
            return responseMappingSetter.Use(extraBuilders.Concat(new[] { builder }));
        }

        public static INClientResponseMappingSelector<TRequest, TResponse> Use<TRequest, TResponse>(
            this INClientResponseMappingSetter<TRequest, TResponse> responseMappingSetter,
            IResponseMapperProvider<IRequest, IResponse> provider, params IResponseMapperProvider<IRequest, IResponse>[] extraProviders)
        {
            return responseMappingSetter.Use(extraProviders.Concat(new[] { provider }));
        }

        public static INClientResponseMappingSelector<TRequest, TResponse> Use<TRequest, TResponse>(
            this INClientTransportResponseMappingSetter<TRequest, TResponse> transportResponseMappingSetter,
            IResponseMapper<TRequest, TResponse> builder, params IResponseMapper<TRequest, TResponse>[] extraBuilders)
        {
            return transportResponseMappingSetter.Use(extraBuilders.Concat(new[] { builder }));
        }

        public static INClientResponseMappingSelector<TRequest, TResponse> Use<TRequest, TResponse>(
            this INClientTransportResponseMappingSetter<TRequest, TResponse> transportResponseMappingSetter,
            IResponseMapperProvider<TRequest, TResponse> provider, params IResponseMapperProvider<TRequest, TResponse>[] extraProviders)
        {
            return transportResponseMappingSetter.Use(extraProviders.Concat(new[] { provider }));
        }
    }
}
