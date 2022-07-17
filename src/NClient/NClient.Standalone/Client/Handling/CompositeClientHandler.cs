using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NClient.Providers.Handling;

namespace NClient.Standalone.Client.Handling
{
    internal class CompositeClientHandler<TRequest, TResponse> : IClientHandler<TRequest, TResponse>
    {
        private readonly IReadOnlyCollection<IClientHandler<TRequest, TResponse>> _clientHandlers;

        public CompositeClientHandler(IReadOnlyCollection<IClientHandler<TRequest, TResponse>> clientHandlers)
        {
            _clientHandlers = clientHandlers
                .OrderByDescending(x => x is IOrderedClientHandler<TRequest, TResponse>)
                .ThenBy(x => (x as IOrderedClientHandler<TRequest, TResponse>)?.Order)
                .ToArray();
        }

        public async Task<TRequest> HandleRequestAsync(TRequest request, CancellationToken cancellationToken)
        {
            var handledHttpRequest = request;
            foreach (var clientHandler in _clientHandlers)
            {
                handledHttpRequest = await clientHandler
                    .HandleRequestAsync(handledHttpRequest, cancellationToken)
                    .ConfigureAwait(false);
            }

            return handledHttpRequest;
        }

        public async Task<TResponse> HandleResponseAsync(TResponse response, CancellationToken cancellationToken)
        {
            var handledHttpResponse = response;
            foreach (var clientHandler in _clientHandlers)
            {
                handledHttpResponse = await clientHandler
                    .HandleResponseAsync(handledHttpResponse, cancellationToken)
                    .ConfigureAwait(false);
            }

            return handledHttpResponse;
        }
    }
}
