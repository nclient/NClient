// ReSharper disable once CheckNamespace

namespace NClient
{
    public interface IClientGallery
    {
        INClientBasicBuilder GetBasic();
        INClientStandardBuilder GetStandard();
        INClientBuilder GetCustom();
    }
    
    public class ClientGallery : IClientGallery
    {
        public INClientBasicBuilder GetBasic() => new NClientBasicBuilder();
        public INClientStandardBuilder GetStandard() => new NClientStandardBuilder();
        public INClientBuilder GetCustom() => new NClientBuilder();
    }
}
