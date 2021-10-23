namespace NClient.Abstractions.Providers.Transport
{
    public interface IHttpParameter
    {
        string Name { get; }
        object? Value { get; }
    }
}
