using NClient.Common.Helpers;

namespace NClient.Abstractions.HttpClients
{
    public record HttpHeader
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
