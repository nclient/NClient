// ReSharper disable once CheckNamespace

namespace NClient
{
    public interface IClientFactoryGallery
    {
        INClientBasicFactoryBuilder GetBasic();
        INClientStandardFactoryBuilder GetStandard();
        INClientFactoryBuilder GetCustom();
    }
    
    public class ClientFactoryGallery : IClientFactoryGallery
    {
        public INClientBasicFactoryBuilder GetBasic() => new NClientBasicFactoryBuilder();
        public INClientStandardFactoryBuilder GetStandard() => new NClientStandardFactoryBuilder();
        public INClientFactoryBuilder GetCustom() => new NClientFactoryBuilder();
    }
}
