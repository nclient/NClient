// ReSharper disable once CheckNamespace

namespace NClient
{
    public interface IClientFactoryGallery
    {
        INClientFactoryRestBuilder GetRest();
        INClientFactoryBuilder GetCustom();
    }
    
    public class ClientFactoryGallery : IClientFactoryGallery
    {
        public INClientFactoryRestBuilder GetRest() => new NClientFactoryRestBuilder();
        public INClientFactoryBuilder GetCustom() => new NClientFactoryBuilder();
    }
}
