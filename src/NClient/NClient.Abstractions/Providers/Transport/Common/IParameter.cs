// ReSharper disable once CheckNamespace

namespace NClient.Providers.Transport
{
    /// <summary>Represents a parameter of a request or response.</summary>
    public interface IParameter
    {
        /// <summary>Gets parameter name.</summary>
        string Name { get; }
        
        /// <summary>Gets parameter value.</summary>
        object? Value { get; }
    }
}
