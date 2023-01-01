using NClient.Providers;
using NClient.Providers.Handling;
using NClient.Providers.Mapping;
using NClient.Providers.Resilience;
using NClient.Providers.Transport;
using NClient.Providers.Validation;
using NClient.Providers.Transport.Common;

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
        private readonly IResponseMapperProvider<IRequest, IResponse> _responseMapperProvider;
        private readonly IResponseMapperProvider<TRequest, TResponse> _transportResponseMapperProvider;
        private readonly IResponseValidatorProvider<TRequest, TResponse> _responseValidatorProvider;
        private readonly IToolset _toolset;
        private IPipelineCanceller _pipelineCanceller;

        public IPipelineCanceller PipelineCanceller
        {
            get { return _pipelineCanceller; }
            set { _pipelineCanceller = value; }
        }

        public TransportNClientFactory(
            ITransportProvider<TRequest, TResponse> transportProvider,
            ITransportRequestBuilderProvider<TRequest, TResponse> transportRequestBuilderProvider,
            IResponseBuilderProvider<TRequest, TResponse> responseBuilderProvider,
            IClientHandlerProvider<TRequest, TResponse> clientHandlerProvider,
            IResiliencePolicyProvider<TRequest, TResponse> resiliencePolicyProvider,
            IResponseMapperProvider<IRequest, IResponse> responseMapperProvider,
            IResponseMapperProvider<TRequest, TResponse> transportResponseMapperProvider,
            IResponseValidatorProvider<TRequest, TResponse> responseValidatorProvider,
            IToolset toolset,
            IPipelineCanceller pipelineCanceller)
        {
            _transportProvider = transportProvider;
            _transportRequestBuilderProvider = transportRequestBuilderProvider;
            _responseBuilderProvider = responseBuilderProvider;
            _clientHandlerProvider = clientHandlerProvider;
            _resiliencePolicyProvider = resiliencePolicyProvider;
            _responseMapperProvider = responseMapperProvider;
            _transportResponseMapperProvider = transportResponseMapperProvider;
            _responseValidatorProvider = responseValidatorProvider;
            _toolset = toolset;
            _pipelineCanceller = pipelineCanceller;
        }

        public ITransportNClient<TRequest, TResponse> Create()
        {
            return new TransportNClient<TRequest, TResponse>(
                _toolset.Serializer,
                _transportProvider.Create(_toolset),
                _transportRequestBuilderProvider.Create(_toolset, _pipelineCanceller),
                _responseBuilderProvider.Create(_toolset),
                _clientHandlerProvider.Create(_toolset),
                _resiliencePolicyProvider.Create(_toolset),
                _responseMapperProvider.Create(_toolset),
                _transportResponseMapperProvider.Create(_toolset),
                _responseValidatorProvider.Create(_toolset),
                _toolset.Logger);
        }
    }
}
