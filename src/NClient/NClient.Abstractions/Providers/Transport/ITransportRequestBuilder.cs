﻿using System.Threading;
using System.Threading.Tasks;

namespace NClient.Providers.Transport
{
    // TODO: doc
    // ReSharper disable once UnusedTypeParameter
    public interface ITransportRequestBuilder<TRequest, TResponse>
    {
        Task<TRequest> BuildAsync(IRequest request, CancellationToken cancellationToken);
    }
}
