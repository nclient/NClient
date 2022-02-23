using NClient.Common.Helpers;

// ReSharper disable once CheckNamespace
namespace NClient.Providers.Transport
{
    /// <summary>Represents a parameter of a request or response.</summary>
    public class Parameter : IParameter
    {
        /// <summary>Gets parameter name.</summary>
        public string Name { get; }
        
        /// <summary>Gets parameter value.</summary>
        public object? Value { get; }

        public Parameter(string name, object? value)
        {
            Ensure.IsNotNullOrEmpty(name, nameof(name));

            Name = name;
            Value = value;
        }
    }
}
