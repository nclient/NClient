using Castle.DynamicProxy;

namespace NClient.Standalone.ClientProxy.ClientGeneration
{
    internal interface IClientGenerator
    {
        TClient CreateClient<TClient>(IAsyncInterceptor asyncInterceptor);
    }

    internal class ClientGenerator : IClientGenerator
    {
        private readonly IProxyGenerator _proxyGenerator;

        public ClientGenerator(IProxyGenerator proxyGenerator)
        {
            _proxyGenerator = proxyGenerator;
        }

        public TClient CreateClient<TClient>(IAsyncInterceptor asyncInterceptor)
        {
            return (TClient)_proxyGenerator.CreateInterfaceProxyWithoutTarget(
                interfaceToProxy: typeof(TClient),
                additionalInterfacesToProxy: new[] { typeof(IResilienceNClient<TClient>), typeof(IHttpNClient<TClient>) },
                interceptors: asyncInterceptor.ToInterceptor());
        }
    }
}
