using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using NClient.Providers.Handling;
using NClient.Providers.Resilience;
using NClient.Providers.Results;
using NClient.Providers.Serialization;
using NClient.Providers.Transport;
using NClient.Providers.Validation;

namespace NClient.Standalone.Client
{
    internal interface IHttpNClientFactory<TRequest, TResponse>
    {
        IHttpNClient<TRequest, TResponse> Create();
    }

    internal class HttpNClientFactory<TRequest, TResponse> : IHttpNClientFactory<TRequest, TResponse>
    {
        private readonly ISerializerProvider _serializerProvider;
        private readonly ITransportProvider<TRequest, TResponse> _transportProvider;
        private readonly ITransportMessageBuilderProvider<TRequest, TResponse> _transportMessageBuilderProvider;
        private readonly IClientHandlerProvider<TRequest, TResponse> _clientHandlerProvider;
        private readonly IResiliencePolicyProvider<TRequest, TResponse> _resiliencePolicyProvider;
        private readonly IEnumerable<IResultBuilderProvider<IResponse>> _resultBuilderProviders;
        private readonly IEnumerable<IResultBuilderProvider<TResponse>> _typedResultBuilderProviders;
        private readonly IResponseValidatorProvider<TRequest, TResponse> _responseValidatorProvider;
        private readonly ILogger? _logger;

        public HttpNClientFactory(
            ISerializerProvider serializerProvider,
            ITransportProvider<TRequest, TResponse> transportProvider,
            ITransportMessageBuilderProvider<TRequest, TResponse> transportMessageBuilderProvider,
            IClientHandlerProvider<TRequest, TResponse> clientHandlerProvider,
            IResiliencePolicyProvider<TRequest, TResponse> resiliencePolicyProvider,
            IEnumerable<IResultBuilderProvider<IResponse>> resultBuilderProviders,
            IEnumerable<IResultBuilderProvider<TResponse>> typedResultBuilderProviders,
            IResponseValidatorProvider<TRequest, TResponse> responseValidatorProvider,
            ILogger? logger)
        {
            _serializerProvider = serializerProvider;
            _transportProvider = transportProvider;
            _transportMessageBuilderProvider = transportMessageBuilderProvider;
            _clientHandlerProvider = clientHandlerProvider;
            _resiliencePolicyProvider = resiliencePolicyProvider;
            _resultBuilderProviders = resultBuilderProviders;
            _typedResultBuilderProviders = typedResultBuilderProviders;
            _responseValidatorProvider = responseValidatorProvider;
            _logger = logger;
        }

        public IHttpNClient<TRequest, TResponse> Create()
        {
            var serializer = _serializerProvider.Create();
            
            return new HttpNClient<TRequest, TResponse>(
                serializer,
                _transportProvider.Create(serializer),
                _transportMessageBuilderProvider.Create(serializer),
                _clientHandlerProvider.Create(),
                _resiliencePolicyProvider.Create(),
                _resultBuilderProviders.Select(x => x.Create()),
                _typedResultBuilderProviders.Select(x => x.Create()),
                _responseValidatorProvider.Create(),
                _logger);
        }
    }
}
