using Castle.DynamicProxy;
using NClient.Standalone.Exceptions.Factories;

namespace NClient.Standalone.ClientProxy.Generation
{
    internal interface IClientProxyGenerator
    {
        TClient CreateClient<TClient>(IAsyncInterceptor asyncInterceptor);
    }

    internal class ClientProxyGenerator : IClientProxyGenerator
    {
        private readonly IProxyGenerator _proxyGenerator;
        private readonly IClientValidationExceptionFactory _clientValidationExceptionFactory;

        public ClientProxyGenerator(
            IProxyGenerator proxyGenerator,
            IClientValidationExceptionFactory clientValidationExceptionFactory)
        {
            _proxyGenerator = proxyGenerator;
            _clientValidationExceptionFactory = clientValidationExceptionFactory;
        }

        public TClient CreateClient<TClient>(IAsyncInterceptor asyncInterceptor)
        {
            if (!typeof(TClient).IsInterface)
                throw _clientValidationExceptionFactory.ClientTypeIsNotInterface(typeof(TClient));
            
            return (TClient) _proxyGenerator.CreateInterfaceProxyWithoutTarget(
                interfaceToProxy: typeof(TClient),
                additionalInterfacesToProxy: new[] { typeof(IResilienceNClient<TClient>), typeof(ITransportNClient<TClient>) },
                interceptors: asyncInterceptor.ToInterceptor());
        }
    }
}
