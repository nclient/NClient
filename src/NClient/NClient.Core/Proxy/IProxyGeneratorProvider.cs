using Castle.DynamicProxy;

namespace NClient.Core.Proxy
{
    public interface IProxyGeneratorProvider
    {
        IProxyGenerator Value { get; }
    }
}
