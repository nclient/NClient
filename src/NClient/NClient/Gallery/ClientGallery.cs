// ReSharper disable once CheckNamespace

namespace NClient
{
    public interface IClientGallery
    {
        IBasicNClientBuilder GetBasic();
        IStandardNClientBuilder GetStandard();
        INClientAdvancedBuilder GetCustom();
    }
    
    public class ClientGallery : IClientGallery
    {
        public IBasicNClientBuilder GetBasic() => new BasicNClientBuilder();
        public IStandardNClientBuilder GetStandard() => new StandardNClientBuilder();
        public INClientAdvancedBuilder GetCustom() => new CustomNClientBuilder();
    }
}
