using Castle.DynamicProxy;

namespace NClient.Core.Proxy
{
    internal class SingletonProxyGeneratorProvider : IProxyGeneratorProvider
    {
        private static readonly IProxyGenerator ProxyGenerator = new ProxyGenerator();

        public IProxyGenerator Value => ProxyGenerator;
    }
}
