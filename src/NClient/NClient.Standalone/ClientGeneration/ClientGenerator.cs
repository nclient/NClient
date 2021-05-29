using Castle.DynamicProxy;
using NClient.Abstractions.Clients;

namespace NClient.ClientGeneration
{
    public interface IClientGenerator
    {
        TInterface CreateClient<TInterface>(IAsyncInterceptor asyncInterceptor);
    }

    public class ClientGenerator : IClientGenerator
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