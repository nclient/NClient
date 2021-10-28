// ReSharper disable once CheckNamespace

namespace NClient.Providers.Transport
{
    public interface IParameter
    {
        string Name { get; }
        object? Value { get; }
    }
}
