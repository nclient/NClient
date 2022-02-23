using Castle.DynamicProxy;

namespace NClient.Core.Proxy
{
    internal interface IProxyGeneratorProvider
    {
        IProxyGenerator Value { get; }
    }
}
