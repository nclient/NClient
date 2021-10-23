namespace NClient.Abstractions.Providers.Transport
{
    public interface IHttpHeader
    {
        string Name { get; }
        string Value { get; }
    }
}
