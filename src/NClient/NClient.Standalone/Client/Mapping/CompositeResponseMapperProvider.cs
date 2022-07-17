using System.Collections.Generic;
using System.Linq;
using NClient.Providers;
using NClient.Providers.Mapping;

namespace NClient.Standalone.Client.Mapping
{
    internal class CompositeResponseMapperProvider<TRequest, TResponse> : IResponseMapperProvider<TRequest, TResponse>
    {
        private readonly IReadOnlyCollection<IResponseMapperProvider<TRequest, TResponse>> _responseMapperProviders;
        
        public CompositeResponseMapperProvider(IReadOnlyCollection<IResponseMapperProvider<TRequest, TResponse>> responseMapperProviders)
        {
            _responseMapperProviders = responseMapperProviders;
        }
        
        public IResponseMapper<TRequest, TResponse> Create(IToolset toolset)
        {
            return new CompositeResponseMapper<TRequest, TResponse>(_responseMapperProviders
                .Select(x => x.Create(toolset))
                .ToArray());
        }
    }
}
