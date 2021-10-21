namespace NClient.Abstractions.Providers.HttpClient
{
    public interface IHttpHeader
    {
        string Name { get; }
        string Value { get; }
    }
}
