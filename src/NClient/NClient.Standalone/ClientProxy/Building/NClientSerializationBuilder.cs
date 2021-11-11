﻿using NClient.Common.Helpers;
using NClient.Providers.Serialization;
using NClient.Standalone.ClientProxy.Building.Context;

namespace NClient.Standalone.ClientProxy.Building
{
    internal class NClientSerializationBuilder<TClient, TRequest, TResponse> : INClientSerializationBuilder<TClient, TRequest, TResponse>
        where TClient : class
    {
        private readonly BuilderContext<TRequest, TResponse> _context;
        
        public NClientSerializationBuilder(BuilderContext<TRequest, TResponse> context)
        {
            _context = context;
        }
        
        public INClientOptionalBuilder<TClient, TRequest, TResponse> UsingCustomSerializer(ISerializerProvider serializerProvider)
        {
            Ensure.IsNotNull(serializerProvider, nameof(serializerProvider));
            
            return new NClientOptionalBuilder<TClient, TRequest, TResponse>(_context
                .WithSerializer(serializerProvider));
        }
    }
}
