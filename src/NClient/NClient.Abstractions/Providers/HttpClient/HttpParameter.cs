using NClient.Common.Helpers;

namespace NClient.Abstractions.Providers.HttpClient
{
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
