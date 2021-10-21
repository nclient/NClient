namespace NClient.Abstractions.Providers.HttpClient
{
    public interface IHttpParameter
    {
        string Name { get; }
        object? Value { get; }
    }
}
