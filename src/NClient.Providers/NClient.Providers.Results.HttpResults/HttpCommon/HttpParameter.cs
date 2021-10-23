using NClient.Common.Helpers;

// ReSharper disable once CheckNamespace
namespace NClient.Providers.Results.HttpResults
{
    public interface IHttpParameter
    {
        string Name { get; }
        object? Value { get; }
    }
    
    /// <summary>
    /// The container for HTTP header data.
    /// </summary>
    public class HttpParameter : IHttpParameter
    {
        public string Name { get; }
        public object? Value { get; }

        public HttpParameter(string name, object? value)
        {
            Ensure.IsNotNullOrEmpty(name, nameof(name));

            Name = name;
            Value = value;
        }
    }
}
