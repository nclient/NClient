using System.Collections.Generic;
using NClient.Providers.Mapping;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public interface INClientTransportResponseMappingSetter<TRequest, TResponse>
    {
        INClientResponseMappingSelector<TRequest, TResponse> Use(IEnumerable<IResponseMapper<TRequest, TResponse>> mappers);
        
        INClientResponseMappingSelector<TRequest, TResponse> Use(IEnumerable<IResponseMapperProvider<TRequest, TResponse>> providers);
    }
}
