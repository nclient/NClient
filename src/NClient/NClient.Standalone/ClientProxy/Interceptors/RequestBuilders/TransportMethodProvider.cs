using System;
using NClient.Annotations.Methods;
using NClient.Providers.Transport;
using NClient.Standalone.Exceptions.Factories;

namespace NClient.Standalone.ClientProxy.Interceptors.RequestBuilders
{
    internal interface ITransportMethodProvider
    {
        RequestType Get(MethodAttribute methodAttribute);
    }

    internal class TransportMethodProvider : ITransportMethodProvider
    {
        private readonly IClientValidationExceptionFactory _clientValidationExceptionFactory;

        public TransportMethodProvider(IClientValidationExceptionFactory clientValidationExceptionFactory)
        {
            _clientValidationExceptionFactory = clientValidationExceptionFactory;
        }

        public RequestType Get(MethodAttribute methodAttribute)
        {
            return methodAttribute switch
            {
                GetMethodAttribute => RequestType.Get,
                HeadMethodAttribute => RequestType.Head,
                PostMethodAttribute => RequestType.Post,
                PutMethodAttribute => RequestType.Put,
                DeleteMethodAttribute => RequestType.Delete,
                OptionsMethodAttribute => RequestType.Options,
                #if !NETSTANDARD2_0
                PatchMethodAttribute => RequestType.Patch,
                #endif
                { } => throw _clientValidationExceptionFactory.MethodAttributeNotSupported(methodAttribute.GetType().Name),
                _ => throw new ArgumentNullException(nameof(methodAttribute))
            };
        }
    }
}
