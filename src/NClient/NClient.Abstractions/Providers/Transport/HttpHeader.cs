using NClient.Common.Helpers;

namespace NClient.Providers.Transport
{
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
