// ReSharper disable once CheckNamespace

namespace NClient.Providers.Transport
{
    public interface IHeader
    {
        string Name { get; }
        string Value { get; }
    }
}
