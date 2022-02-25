// ReSharper disable once CheckNamespace

namespace NClient
{
    /// <summary>The container of builder sets for creating pre-configured clients and client factories.</summary>
    public interface INClientGallery
    {
        /// <summary>Gets the container of builder for creating pre-configured clients.</summary>
        IClientGallery Clients { get; }
        
        /// <summary>Gets the container of builder sets for creating pre-configured client factories.</summary>
        IClientFactoryGallery ClientFactories { get; }
    }
    
    /// <summary>The container of factory sets for creating pre-configured clients and client factories.</summary>
    public class NClientGallery : INClientGallery
    {
        /// <summary>Gets the container of builder for creating pre-configured clients.</summary>
        public static IClientGallery Clients { get; } = new ClientGallery();
        
        /// <summary>Gets the container of builder sets for creating pre-configured client factories.</summary>
        public static IClientFactoryGallery ClientFactories { get; } = new ClientFactoryGallery();
        
        IClientGallery INClientGallery.Clients { get; } = Clients;
        IClientFactoryGallery INClientGallery.ClientFactories { get; } = ClientFactories;
    }
}
