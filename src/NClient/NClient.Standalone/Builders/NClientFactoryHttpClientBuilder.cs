﻿using NClient.Abstractions.Builders;
using NClient.Abstractions.HttpClients;
using NClient.Builders.Context;
using NClient.Common.Helpers;

namespace NClient.Builders
{
    internal class NClientFactoryHttpClientBuilder : INClientFactoryHttpClientBuilder
    {
        private readonly string _factoryName;
        
        public NClientFactoryHttpClientBuilder(string factoryName)
        {
            _factoryName = factoryName;
        }
        
        public INClientFactorySerializerBuilder<TRequest, TResponse> UsingCustomHttpClient<TRequest, TResponse>(
            IHttpClientProvider<TRequest, TResponse> httpClientProvider, 
            IHttpMessageBuilderProvider<TRequest, TResponse> httpMessageBuilderProvider,
            IHttpClientExceptionFactory<TRequest, TResponse> httpClientExceptionFactory)
        {
            Ensure.IsNotNull(httpClientProvider, nameof(httpClientProvider));
            Ensure.IsNotNull(httpMessageBuilderProvider, nameof(httpMessageBuilderProvider));
            Ensure.IsNotNull(httpClientExceptionFactory, nameof(httpClientExceptionFactory));

            var context = new CustomizerContext<TRequest, TResponse>();
            
            context.SetHttpClientProvider(httpClientProvider, httpMessageBuilderProvider, httpClientExceptionFactory);
            return new NClientFactorySerializerBuilder<TRequest, TResponse>(_factoryName, context);
        }
    }
}
