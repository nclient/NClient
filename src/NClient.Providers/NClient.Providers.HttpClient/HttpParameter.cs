namespace NClient.Providers.HttpClient
{
    public class HttpParameter
    {
        public string Name { get; }
        public object? Value { get; }

        public HttpParameter(string name, object? value)
        {
            Name = name;
            Value = value;
        }
    }
}
