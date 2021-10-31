﻿using NClient.Providers.Serialization;

namespace NClient.Providers.Transport
{
    public interface ITransportRequestBuilderProvider<TRequest, TResponse>
    {
        ITransportRequestBuilder<TRequest, TResponse> Create(ISerializer serializer);
    }
}