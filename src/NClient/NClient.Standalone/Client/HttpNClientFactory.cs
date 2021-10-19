using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using NClient.Abstractions.Handling;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;
using NClient.Abstractions.Results;
using NClient.Abstractions.Serialization;
using NClient.Standalone.Client.Validation;

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
        private readonly IResponseValidator<TRequest, TResponse> _responseValidator;
        private readonly ILogger? _logger;

        public HttpNClientFactory(
            ISerializerProvider serializerProvider,
            IHttpClientProvider<TRequest, TResponse> httpClientProvider,
            IHttpMessageBuilderProvider<TRequest, TResponse> httpMessageBuilderProvider,
            IClientHandlerProvider<TRequest, TResponse> clientHandlerProvider,
            IResiliencePolicyProvider<TRequest, TResponse> resiliencePolicyProvider,
            IEnumerable<IResultBuilderProvider<IHttpResponse>> resultBuilderProviders,
            IEnumerable<IResultBuilderProvider<TResponse>> typedResultBuilderProviders,
            IResponseValidator<TRequest, TResponse> responseValidator,
            ILogger? logger)
        {
            _serializerProvider = serializerProvider;
            _httpClientProvider = httpClientProvider;
            _httpMessageBuilderProvider = httpMessageBuilderProvider;
            _clientHandlerProvider = clientHandlerProvider;
            _resiliencePolicyProvider = resiliencePolicyProvider;
            _resultBuilderProviders = resultBuilderProviders;
            _typedResultBuilderProviders = typedResultBuilderProviders;
            _responseValidator = responseValidator;
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
                _responseValidator,
                _logger);
        }
    }
}
