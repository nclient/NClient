using System.Collections.Generic;
using NClient.Providers.Mapping;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public interface INClientTransportResponseMappingSetter<TRequest, TResponse>
    {
        // TODO: doc
        INClientResponseMappingSelector<TRequest, TResponse> Use(IEnumerable<IResponseMapper<TRequest, TResponse>> builders);
        
        INClientResponseMappingSelector<TRequest, TResponse> Use(IEnumerable<IResponseMapperProvider<TRequest, TResponse>> providers);
    }
}
