namespace NClient.Providers.HttpClient.Abstractions
{
    public interface IHttpClientProvider
    {
        IHttpClient Create();
    }
}
