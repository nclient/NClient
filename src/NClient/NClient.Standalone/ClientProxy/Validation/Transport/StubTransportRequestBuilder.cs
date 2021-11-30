﻿using System.Threading;
using System.Threading.Tasks;
using NClient.Providers.Transport;

namespace NClient.Standalone.ClientProxy.Validation.Transport
{
    internal class StubTransportRequestBuilder : ITransportRequestBuilder<IRequest, IResponse>
    {
        public Task<IRequest> BuildAsync(IRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(request);
        }
        public Task<IResponse> BuildResponseAsync(IRequest request, 
            IResponseContext<IRequest, IResponse> responseContext, CancellationToken cancellationToken)
        {
            return Task.FromResult(responseContext.Response);
        }
    }
}
