using NClient.Providers.Api;

// ReSharper disable once CheckNamespace
namespace NClient
{
    /// <summary>Setter a custom provider of the request builder that turns a method call into a request.</summary>
    public interface INClientApiBuilder<TClient> where TClient : class
    {
        /// <summary>Sets a custom provider of the request builder that turns a method call into a request.</summary>
        /// <param name="requestBuilderProvider">The provider of the request builder that turns a method call into a request.</param>
        INClientTransportBuilder<TClient> UsingCustomApi(IRequestBuilderProvider requestBuilderProvider);
    }
}
