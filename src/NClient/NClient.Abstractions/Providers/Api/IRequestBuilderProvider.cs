namespace NClient.Providers.Api
{
    public interface IRequestBuilderProvider
    {
        IRequestBuilder Create(IToolset toolset);
    }
}
