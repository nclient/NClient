// ReSharper disable once CheckNamespace

namespace NClient
{
    public interface IClientGallery
    {
        INClientBasicBuilder GetBasic();
        INClientStandardBuilder GetStandard();
        INClientAdvancedBuilder GetAdvanced();
    }
    
    public class ClientGallery : IClientGallery
    {
        public INClientBasicBuilder GetBasic() => new NClientBasicBuilder();
        public INClientStandardBuilder GetStandard() => new NClientStandardBuilder();
        public INClientAdvancedBuilder GetAdvanced() => new NClientAdvancedBuilder();
    }
}
