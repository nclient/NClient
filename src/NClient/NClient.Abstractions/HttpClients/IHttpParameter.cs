namespace NClient.Abstractions.HttpClients
{
    public interface IHttpParameter
    {
        string Name { get; }
        object? Value { get; }
    }
}
