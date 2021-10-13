using System.Collections.Generic;
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
        private readonly IClientHandler<TRequest, TResponse> _clientHandler;
        private readonly IResiliencePolicyProvider<TRequest, TResponse> _resiliencePolicyProvider;
        private readonly IEnumerable<IResultBuilder<IHttpResponse>> _resultBuilders;
        private readonly IResponseValidator<TRequest, TResponse> _responseValidator;
        private readonly ILogger? _logger;

        public HttpNClientFactory(
            ISerializerProvider serializerProvider,
            IHttpClientProvider<TRequest, TResponse> httpClientProvider,
            IHttpMessageBuilderProvider<TRequest, TResponse> httpMessageBuilderProvider,
            IClientHandler<TRequest, TResponse> clientHandler,
            IResiliencePolicyProvider<TRequest, TResponse> resiliencePolicyProvider,
            IEnumerable<IResultBuilder<IHttpResponse>> resultBuilders,
            IResponseValidator<TRequest, TResponse> responseValidator,
            ILogger? logger)
        {
            _serializerProvider = serializerProvider;
            _httpClientProvider = httpClientProvider;
            _httpMessageBuilderProvider = httpMessageBuilderProvider;
            _clientHandler = clientHandler;
            _resiliencePolicyProvider = resiliencePolicyProvider;
            _resultBuilders = resultBuilders;
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
                _clientHandler,
                _resiliencePolicyProvider.Create(),
                _resultBuilders,
                _responseValidator,
                _logger);
        }
    }
}
