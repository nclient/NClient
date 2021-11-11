using NClient.Providers;
using NClient.Providers.Mapping;

namespace NClient.Standalone.Client.Mapping
{
    internal class ResponseMapperProvider<TRequest, TResponse> : IResponseMapperProvider<TRequest, TResponse>
    {
        private readonly IResponseMapper<TRequest, TResponse> _responseMapper;
        
        public ResponseMapperProvider(IResponseMapper<TRequest, TResponse> responseMapper)
        {
            _responseMapper = responseMapper;
        }
        
        public IResponseMapper<TRequest, TResponse> Create(IToolSet toolset)
        {
            return _responseMapper;
        }
    }
}
