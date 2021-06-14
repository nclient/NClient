using Castle.DynamicProxy;
using NClient.Abstractions.Clients;

namespace NClient.ClientGeneration
{
    internal interface IClientGenerator
    {
        TInterface CreateClient<TInterface>(IAsyncInterceptor asyncInterceptor);
    }

    internal class ClientGenerator : IClientGenerator
    {
        private readonly IProxyGenerator _proxyGenerator;

        public ClientGenerator(IProxyGenerator proxyGenerator)
        {
            _proxyGenerator = proxyGenerator;
        }

        public TInterface CreateClient<TInterface>(IAsyncInterceptor asyncInterceptor)
        {
            return (TInterface)_proxyGenerator.CreateInterfaceProxyWithoutTarget(
                interfaceToProxy: typeof(TInterface),
                additionalInterfacesToProxy: new[] { typeof(IResilienceNClient<TInterface>), typeof(IHttpNClient<TInterface>) },
                interceptors: asyncInterceptor.ToInterceptor());
        }
    }
}