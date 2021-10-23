using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using NClient.Abstractions.Providers.Handling;
using NClient.Abstractions.Providers.Resilience;
using NClient.Abstractions.Providers.Results;
using NClient.Abstractions.Providers.Serialization;
using NClient.Abstractions.Providers.Transport;
using NClient.Abstractions.Providers.Validation;

namespace NClient.Standalone.Client
{
    internal interface IHttpNClientFactory<TRequest, TResponse>
    {
        IHttpNClient<TRequest, TResponse> Create();
    }

    internal class HttpNClientFactory<TRequest, TResponse> : IHttpNClientFactory<TRequest, TResponse>
    {
        private readonly ISerializerProvider _serializerProvider;
        private readonly IHttpClientProvider<TRequest, TResponse> _httpClientProvider;
        private readonly IHttpMessageBuilderProvider<TRequest, TResponse> _httpMessageBuilderProvider;
        private readonly IClientHandlerProvider<TRequest, TResponse> _clientHandlerProvider;
        private readonly IResiliencePolicyProvider<TRequest, TResponse> _resiliencePolicyProvider;
        private readonly IEnumerable<IResultBuilderProvider<IHttpResponse>> _resultBuilderProviders;
        private readonly IEnumerable<IResultBuilderProvider<TResponse>> _typedResultBuilderProviders;
        private readonly IResponseValidatorProvider<TRequest, TResponse> _responseValidatorProvider;
        private readonly ILogger? _logger;

        public HttpNClientFactory(
            ISerializerProvider serializerProvider,
            IHttpClientProvider<TRequest, TResponse> httpClientProvider,
            IHttpMessageBuilderProvider<TRequest, TResponse> httpMessageBuilderProvider,
            IClientHandlerProvider<TRequest, TResponse> clientHandlerProvider,
            IResiliencePolicyProvider<TRequest, TResponse> resiliencePolicyProvider,
            IEnumerable<IResultBuilderProvider<IHttpResponse>> resultBuilderProviders,
            IEnumerable<IResultBuilderProvider<TResponse>> typedResultBuilderProviders,
            IResponseValidatorProvider<TRequest, TResponse> responseValidatorProvider,
            ILogger? logger)
        {
            _serializerProvider = serializerProvider;
            _httpClientProvider = httpClientProvider;
            _httpMessageBuilderProvider = httpMessageBuilderProvider;
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
                _httpClientProvider.Create(serializer),
                _httpMessageBuilderProvider.Create(serializer),
                _clientHandlerProvider.Create(),
                _resiliencePolicyProvider.Create(),
                _resultBuilderProviders.Select(x => x.Create()),
                _typedResultBuilderProviders.Select(x => x.Create()),
                _responseValidatorProvider.Create(),
                _logger);
        }
    }
}
