using System.Collections.Generic;
using System.Linq;
using NClient.Providers;
using NClient.Providers.Handling;
using NClient.Providers.Mapping;
using NClient.Providers.Resilience;
using NClient.Providers.Transport;
using NClient.Providers.Validation;

namespace NClient.Standalone.Client
{
    internal interface ITransportNClientFactory<TRequest, TResponse>
    {
        ITransportNClient<TRequest, TResponse> Create();
    }

    internal class TransportNClientFactory<TRequest, TResponse> : ITransportNClientFactory<TRequest, TResponse>
    {
        private readonly ITransportProvider<TRequest, TResponse> _transportProvider;
        private readonly ITransportRequestBuilderProvider<TRequest, TResponse> _transportRequestBuilderProvider;
        private readonly IResponseBuilderProvider<TRequest, TResponse> _responseBuilderProvider;
        private readonly IClientHandlerProvider<TRequest, TResponse> _clientHandlerProvider;
        private readonly IResiliencePolicyProvider<TRequest, TResponse> _resiliencePolicyProvider;
        private readonly IEnumerable<IResponseMapperProvider<IRequest, IResponse>> _resultBuilderProviders;
        private readonly IEnumerable<IResponseMapperProvider<TRequest, TResponse>> _typedResultBuilderProviders;
        private readonly IResponseValidatorProvider<TRequest, TResponse> _responseValidatorProvider;
        private readonly IToolSet _toolset;

        public TransportNClientFactory(
            ITransportProvider<TRequest, TResponse> transportProvider,
            ITransportRequestBuilderProvider<TRequest, TResponse> transportRequestBuilderProvider,
            IResponseBuilderProvider<TRequest, TResponse> responseBuilderProvider,
            IClientHandlerProvider<TRequest, TResponse> clientHandlerProvider,
            IResiliencePolicyProvider<TRequest, TResponse> resiliencePolicyProvider,
            IEnumerable<IResponseMapperProvider<IRequest, IResponse>> resultBuilderProviders,
            IEnumerable<IResponseMapperProvider<TRequest, TResponse>> typedResultBuilderProviders,
            IResponseValidatorProvider<TRequest, TResponse> responseValidatorProvider,
            IToolSet toolset)
        {
            _transportProvider = transportProvider;
            _transportRequestBuilderProvider = transportRequestBuilderProvider;
            _responseBuilderProvider = responseBuilderProvider;
            _clientHandlerProvider = clientHandlerProvider;
            _resiliencePolicyProvider = resiliencePolicyProvider;
            _resultBuilderProviders = resultBuilderProviders;
            _typedResultBuilderProviders = typedResultBuilderProviders;
            _responseValidatorProvider = responseValidatorProvider;
            _toolset = toolset;
        }

        public ITransportNClient<TRequest, TResponse> Create()
        {
            return new TransportNClient<TRequest, TResponse>(
                _toolset.Serializer,
                _transportProvider.Create(_toolset),
                _transportRequestBuilderProvider.Create(_toolset),
                _responseBuilderProvider.Create(_toolset),
                _clientHandlerProvider.Create(_toolset),
                _resiliencePolicyProvider.Create(_toolset),
                _resultBuilderProviders.Select(x => x.Create(_toolset)),
                _typedResultBuilderProviders.Select(x => x.Create(_toolset)),
                _responseValidatorProvider.Create(_toolset),
                _toolset.Logger);
        }
    }
}
