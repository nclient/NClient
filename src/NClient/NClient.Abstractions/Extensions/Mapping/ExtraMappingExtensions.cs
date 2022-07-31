using System.Linq;
using NClient.Providers.Mapping;
using NClient.Providers.Transport;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class ExtraMappingExtensions
    {
        /// <summary>Sets the mappers that convert NClient responses into custom results.</summary>
        /// <param name="optionalBuilder"></param>
        /// <param name="mapper">The mapper that converts transport responses into custom results.</param>
        /// <param name="extraMappers">The additional mappers that will also be set.</param>
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> WithResponseMapping<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> optionalBuilder,
            IResponseMapper<TRequest, TResponse> mapper, params IResponseMapper<TRequest, TResponse>[] extraMappers) 
            where TClient : class
        {
            return optionalBuilder.WithResponseMapping(extraMappers.Concat(new[] { mapper }));
        }
        
        /// <summary>Sets the mappers that convert NClient responses into custom results.</summary>
        /// <param name="optionalBuilder"></param>
        /// <param name="mapper">The mapper that converts transport responses into custom results.</param>
        /// <param name="extraMappers">The additional mappers that will also be set.</param>
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> WithResponseMapping<TRequest, TResponse>(
            this INClientFactoryOptionalBuilder<TRequest, TResponse> optionalBuilder,
            IResponseMapper<TRequest, TResponse> mapper, params IResponseMapper<TRequest, TResponse>[] extraMappers)
        {
            return optionalBuilder.WithResponseMapping(extraMappers.Concat(new[] { mapper }));
        }

        /// <summary>Sets the mappers that convert NClient responses into custom results.</summary>
        /// <param name="responseMappingSetter"></param>
        /// <param name="mapper">The mapper that converts transport responses into custom results.</param>
        /// <param name="extraMappers">The additional mappers that will also be set.</param>
        public static INClientResponseMappingSelector<TRequest, TResponse> Use<TRequest, TResponse>(
            this INClientResponseMappingSetter<TRequest, TResponse> responseMappingSetter,
            IResponseMapper<IRequest, IResponse> mapper, params IResponseMapper<IRequest, IResponse>[] extraMappers)
        {
            return responseMappingSetter.Use(extraMappers.Concat(new[] { mapper }));
        }

        /// <summary>Sets the providers of mappers that convert NClient responses into custom results.</summary>
        /// <param name="responseMappingSetter"></param>
        /// <param name="provider">The provider of a mapper that converts transport responses into custom results.</param>
        /// <param name="extraProviders">The additional mapper providers that will also be set.</param>
        public static INClientResponseMappingSelector<TRequest, TResponse> Use<TRequest, TResponse>(
            this INClientResponseMappingSetter<TRequest, TResponse> responseMappingSetter,
            IResponseMapperProvider<IRequest, IResponse> provider, params IResponseMapperProvider<IRequest, IResponse>[] extraProviders)
        {
            return responseMappingSetter.Use(extraProviders.Concat(new[] { provider }));
        }

        /// <summary>Sets the mappers that convert NClient responses into custom results.</summary>
        /// <param name="transportResponseMappingSetter"></param>
        /// <param name="mapper">The mapper that converts transport responses into custom results.</param>
        /// <param name="extraMappers">The additional mappers that will also be set.</param>
        public static INClientResponseMappingSelector<TRequest, TResponse> Use<TRequest, TResponse>(
            this INClientTransportResponseMappingSetter<TRequest, TResponse> transportResponseMappingSetter,
            IResponseMapper<TRequest, TResponse> mapper, params IResponseMapper<TRequest, TResponse>[] extraMappers)
        {
            return transportResponseMappingSetter.Use(extraMappers.Concat(new[] { mapper }));
        }

        /// <summary>Sets the providers of mappers that convert NClient responses into custom results.</summary>
        /// <param name="transportResponseMappingSetter"></param>
        /// <param name="provider">The provider of a mapper that converts transport responses into custom results.</param>
        /// <param name="extraProviders">The additional mapper providers that will also be set.</param>
        public static INClientResponseMappingSelector<TRequest, TResponse> Use<TRequest, TResponse>(
            this INClientTransportResponseMappingSetter<TRequest, TResponse> transportResponseMappingSetter,
            IResponseMapperProvider<TRequest, TResponse> provider, params IResponseMapperProvider<TRequest, TResponse>[] extraProviders)
        {
            return transportResponseMappingSetter.Use(extraProviders.Concat(new[] { provider }));
        }
    }
}
