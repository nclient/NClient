namespace NClient.Abstractions.HttpClients
{
    public interface IHttpHeader
    {
        string Name { get; }
        string Value { get; }
    }
}
