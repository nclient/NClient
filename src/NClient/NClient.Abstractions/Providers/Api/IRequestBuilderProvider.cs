namespace NClient.Providers.Api
{
    public interface IRequestBuilderProvider
    {
        IRequestBuilder Create(IToolSet toolset);
    }
}
