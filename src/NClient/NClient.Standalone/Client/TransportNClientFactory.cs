using System.Collections.Generic;
using System.Linq;
using NClient.Providers;
using NClient.Providers.Caching;
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
        private readonly IEnumerable<IResponseMapperProvider<IRequest, IResponse>> _responseMapperProviders;
        private readonly IEnumerable<IResponseMapperProvider<TRequest, TResponse>> _transportResponseMapperProviders;
        private readonly IResponseValidatorProvider<TRequest, TResponse> _responseValidatorProvider;
        private readonly IResponseCacheProvider? _responseCacheProvider;
        private readonly IResponseCacheProvider? _transportResponseCacheProvider;
        private readonly IToolset _toolset;

        public TransportNClientFactory(
            ITransportProvider<TRequest, TResponse> transportProvider,
            ITransportRequestBuilderProvider<TRequest, TResponse> transportRequestBuilderProvider,
            IResponseBuilderProvider<TRequest, TResponse> responseBuilderProvider,
            IClientHandlerProvider<TRequest, TResponse> clientHandlerProvider,
            IResiliencePolicyProvider<TRequest, TResponse> resiliencePolicyProvider,
            IEnumerable<IResponseMapperProvider<IRequest, IResponse>> responseMapperProviders,
            IEnumerable<IResponseMapperProvider<TRequest, TResponse>> transportResponseMapperProviders,
            IResponseValidatorProvider<TRequest, TResponse> responseValidatorProvider,
            IResponseCacheProvider? responseCacheProvider,
            IResponseCacheProvider? transportResponseCacheProvider,
            IToolset toolset)
        {
            _transportProvider = transportProvider;
            _transportRequestBuilderProvider = transportRequestBuilderProvider;
            _responseBuilderProvider = responseBuilderProvider;
            _clientHandlerProvider = clientHandlerProvider;
            _resiliencePolicyProvider = resiliencePolicyProvider;
            _responseMapperProviders = responseMapperProviders;
            _transportResponseMapperProviders = transportResponseMapperProviders;
            _responseValidatorProvider = responseValidatorProvider;
            _responseCacheProvider = responseCacheProvider;
            _transportResponseCacheProvider = transportResponseCacheProvider;
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
                _responseMapperProviders.Select(x => x.Create(_toolset)),
                _transportResponseMapperProviders.Select(x => x.Create(_toolset)),
                _responseValidatorProvider.Create(_toolset),
                _responseCacheProvider?.Create(_toolset),
                _transportResponseCacheProvider?.Create(_toolset),
                _toolset.Logger);
        }
    }
}
