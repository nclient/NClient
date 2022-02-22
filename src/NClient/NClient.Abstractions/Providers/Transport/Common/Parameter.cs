using NClient.Common.Helpers;

// ReSharper disable once CheckNamespace
namespace NClient.Providers.Transport
{
    /// <summary>The container for header data.</summary>
    public class Parameter : IParameter
    {
        public string Name { get; }
        public object? Value { get; }

        public Parameter(string name, object? value)
        {
            Ensure.IsNotNullOrEmpty(name, nameof(name));

            Name = name;
            Value = value;
        }
    }
}
