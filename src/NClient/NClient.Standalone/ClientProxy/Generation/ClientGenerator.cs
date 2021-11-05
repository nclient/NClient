using Castle.DynamicProxy;

namespace NClient.Standalone.ClientProxy.Generation
{
    internal interface IClientProxyGenerator
    {
        TClient CreateClient<TClient>(IAsyncInterceptor asyncInterceptor);
    }

    internal class ClientProxyGenerator : IClientProxyGenerator
    {
        private readonly IProxyGenerator _proxyGenerator;

        public ClientProxyGenerator(IProxyGenerator proxyGenerator)
        {
            _proxyGenerator = proxyGenerator;
        }

        public TClient CreateClient<TClient>(IAsyncInterceptor asyncInterceptor)
        {
            return (TClient) _proxyGenerator.CreateInterfaceProxyWithoutTarget(
                interfaceToProxy: typeof(TClient),
                additionalInterfacesToProxy: new[] { typeof(IResilienceNClient<TClient>), typeof(ITransportNClient<TClient>) },
                interceptors: asyncInterceptor.ToInterceptor());
        }
    }
}
