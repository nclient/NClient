// ReSharper disable once CheckNamespace

namespace NClient
{
    public interface IClientGallery
    {
        INClientRestBuilder GetRest();
        INClientBuilder GetCustom();
    }
    
    public class ClientGallery : IClientGallery
    {
        public INClientRestBuilder GetRest() => new NClientRestBuilder();
        public INClientBuilder GetCustom() => new NClientBuilder();
    }
}
