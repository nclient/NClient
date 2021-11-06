using System.Collections.Generic;
using NClient.Providers.Mapping;
using NClient.Providers.Transport;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public interface INClientResponseMappingSetter<TRequest, TResponse>
    {
        INClientResponseMappingSelector<TRequest, TResponse> Use(IEnumerable<IResponseMapper<IRequest, IResponse>> builders);
        
        INClientResponseMappingSelector<TRequest, TResponse> Use(IEnumerable<IResponseMapperProvider<IRequest, IResponse>> providers);
    }
}
