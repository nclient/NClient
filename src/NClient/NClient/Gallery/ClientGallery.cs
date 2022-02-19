// ReSharper disable once CheckNamespace

namespace NClient
{
    public interface IClientGallery
    {
        IRestNClientBuilder GetRest();
        INClientBuilder GetCustom();
    }
    
    public class ClientGallery : IClientGallery
    {
        public IRestNClientBuilder GetRest() => new RestNClientBuilder();
        public INClientBuilder GetCustom() => new NClientBuilder();
    }
}
