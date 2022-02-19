// ReSharper disable once CheckNamespace

namespace NClient
{
    public interface IClientFactoryGallery
    {
        IRestNClientFactoryBuilder GetRest();
        INClientFactoryBuilder GetCustom();
    }
    
    public class ClientFactoryGallery : IClientFactoryGallery
    {
        public IRestNClientFactoryBuilder GetRest() => new RestNClientFactoryBuilder();
        public INClientFactoryBuilder GetCustom() => new NClientFactoryBuilder();
    }
}
