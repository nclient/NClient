// ReSharper disable once CheckNamespace

namespace NClient
{
    public interface IClientFactoryGallery
    {
        INClientBasicFactoryBuilder GetBasic();
        INClientStandardFactoryBuilder GetStandard();
        INClientAdvancedFactoryBuilder GetCustom();
    }
    
    public class ClientFactoryGallery : IClientFactoryGallery
    {
        public INClientBasicFactoryBuilder GetBasic() => new NClientBasicFactoryBuilder();
        public INClientStandardFactoryBuilder GetStandard() => new NClientStandardFactoryBuilder();
        public INClientAdvancedFactoryBuilder GetCustom() => new NClientAdvancedFactoryBuilder();
    }
}
