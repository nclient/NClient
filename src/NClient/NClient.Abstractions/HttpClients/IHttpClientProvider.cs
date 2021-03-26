namespace NClient.Abstractions.HttpClients
{
    public interface IHttpClientProvider
    {
        IHttpClient Create();
    }
}
