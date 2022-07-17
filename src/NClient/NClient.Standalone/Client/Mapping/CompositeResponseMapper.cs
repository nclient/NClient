using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NClient.Providers.Mapping;
using NClient.Providers.Transport;

namespace NClient.Standalone.Client.Mapping
{
    internal class CompositeResponseMapper<TRequest, TResponse> : IResponseMapper<TRequest, TResponse>
    {
        private readonly IReadOnlyCollection<IResponseMapper<TRequest, TResponse>> _responseMappers;
        
        public CompositeResponseMapper(IReadOnlyCollection<IResponseMapper<TRequest, TResponse>> responseMappers)
        {
            _responseMappers = responseMappers;
        }
        public bool CanMap(Type resultType, IResponseContext<TRequest, TResponse> responseContext)
        {
            return _responseMappers.Any(x => x.CanMap(resultType, responseContext));
        }
        public async Task<object?> MapAsync(Type resultType, IResponseContext<TRequest, TResponse> responseContext, CancellationToken cancellationToken)
        {
            var responseMapper = _responseMappers.First(x => x.CanMap(resultType, responseContext));
            return await responseMapper.MapAsync(resultType, responseContext, cancellationToken).ConfigureAwait(false);
        }
    }
}
