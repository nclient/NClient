namespace NClient.Providers.Api
{
    /// <summary>The provider of the request builder that turns a method call into a request.</summary>
    public interface IRequestBuilderProvider
    {
        /// <summary>Creates the request builder that turns a method call into a request.</summary>
        /// <param name="toolset">Tools that help implement providers.</param>
        IRequestBuilder Create(IToolset toolset);
    }
}
