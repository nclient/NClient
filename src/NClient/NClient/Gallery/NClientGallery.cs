// ReSharper disable once CheckNamespace

namespace NClient
{
    public interface INClientGallery
    {
        IClientGallery Clients { get; }
        IClientFactoryGallery ClientFactories { get; }
    }
    
    public class NClientGallery : INClientGallery
    {
        public static IClientGallery Clients { get; } = new ClientGallery();
        public static IClientFactoryGallery ClientFactories { get; } = new ClientFactoryGallery();
        
        IClientGallery INClientGallery.Clients { get; } = Clients;
        IClientFactoryGallery INClientGallery.ClientFactories { get; } = ClientFactories;
    }
}
