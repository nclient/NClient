using System.Collections.Generic;
using NClient.Providers.Mapping;
using NClient.Providers.Transport;

// ReSharper disable once CheckNamespace
namespace NClient
{
    /// <summary>Setter for custom functionality to mapping NClient requests.</summary>
    /// <typeparam name="TRequest">The type of request that is used in the transport implementation.</typeparam>
    /// <typeparam name="TResponse">The type of response that is used in the transport implementation.</typeparam>
    public interface INClientResponseMappingSetter<TRequest, TResponse>
    {
        /// <summary>Sets a custom mappers that can convert NClient responses into custom ones.</summary>
        /// <param name="mappers">The mappers that convert transport responses into custom results.</param>
        INClientResponseMappingSelector<TRequest, TResponse> Use(IEnumerable<IResponseMapper<IRequest, IResponse>> mappers);

        /// <summary>Sets a providers creating custom mappers that can convert NClient responses into custom ones.</summary>
        /// <param name="providers">The providers of a mappers that convert transport responses into custom results.</param>
        INClientResponseMappingSelector<TRequest, TResponse> Use(IEnumerable<IResponseMapperProvider<IRequest, IResponse>> providers);
    }
}
