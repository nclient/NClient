// ReSharper disable once CheckNamespace

namespace NClient
{
    public interface INClientGallery
    {
        INativeNClientGallery NativeClients { get; }
        INativeNClientFactoryGallery NativeClientFactories { get; }
        ICustomNClientGallery CustomClients { get; }
        ICustomNClientFactoryGallery CustomClientFactories { get; }
    }
    
    public class NClientGallery : INClientGallery
    {
        public static INativeNClientGallery NativeClients { get; } = new NativeNClientGallery();
        public static INativeNClientFactoryGallery NativeClientFactories { get; } = new NativeNClientFactoryGallery();
        public static ICustomNClientGallery CustomClients { get; } = new CustomNClientGallery();
        public static ICustomNClientFactoryGallery CustomClientFactories { get; } = new CustomNClientFactoryGallery();

        INativeNClientGallery INClientGallery.NativeClients { get; } = NativeClients;
        INativeNClientFactoryGallery INClientGallery.NativeClientFactories { get; } = NativeClientFactories;
        ICustomNClientGallery INClientGallery.CustomClients { get; } = CustomClients;
        ICustomNClientFactoryGallery INClientGallery.CustomClientFactories { get; } = CustomClientFactories;
    }
}
