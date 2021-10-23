// ReSharper disable once CheckNamespace

namespace NClient.Providers.Transport
{
    public interface IHttpParameter
    {
        string Name { get; }
        object? Value { get; }
    }
}
