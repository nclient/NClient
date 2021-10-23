// ReSharper disable once CheckNamespace

namespace NClient.Providers.Transport
{
    public interface IHttpHeader
    {
        string Name { get; }
        string Value { get; }
    }
}
