using NClient.Common.Helpers;

// ReSharper disable once CheckNamespace
namespace NClient.Providers.Results.HttpResults
{
    public interface IHttpHeader
    {
        string Name { get; }
        string Value { get; }
    }
    
    /// <summary>
    /// The container for HTTP header data.
    /// </summary>
    public record HttpHeader : IHttpHeader
    {
        public string Name { get; }
        public string Value { get; }

        public HttpHeader(string name, string value)
        {
            Ensure.IsNotNullOrEmpty(name, nameof(name));
            Ensure.IsNotNullOrEmpty(value, nameof(value));

            Name = name;
            Value = value;
        }
    }
}
