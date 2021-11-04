﻿using NClient.Providers.Transport;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public interface INClientAdvancedTransportBuilder<TClient> : INClientTransportBuilder<TClient>
        where TClient : class
    {
        /// <summary>
        /// Sets custom <see cref="ITransportProvider{TRequest,TResponse}"/> used to create instances of <see cref="ITransport{TRequest,TResponse}"/>.
        /// </summary>
        /// <param name="transportProvider">The provider that can create instances of <see cref="ITransport{TRequest,TResponse}"/>.</param>
        /// <param name="transportRequestBuilderProvider">The provider that can create instances of <see cref="ITransportRequestBuilder{TRequest,TResponse}"/>.</param>
        INClientAdvancedSerializerBuilder<TClient, TRequest, TResponse> UsingCustomTransport<TRequest, TResponse>(
            ITransportProvider<TRequest, TResponse> transportProvider,
            ITransportRequestBuilderProvider<TRequest, TResponse> transportRequestBuilderProvider,
            IResponseBuilderProvider<TRequest, TResponse> responseBuilderProvider);
    }
}