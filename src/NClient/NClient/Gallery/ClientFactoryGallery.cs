// ReSharper disable once CheckNamespace

namespace NClient
{
    public interface IClientFactoryGallery
    {
        IBasicNClientFactoryBuilder GetBasic();
        IStandardNClientFactoryBuilder GetStandard();
        INClientFactoryBuilder GetCustom();
    }
    
    public class ClientFactoryGallery : IClientFactoryGallery
    {
        public IBasicNClientFactoryBuilder GetBasic() => new BasicNClientFactoryBuilder();
        public IStandardNClientFactoryBuilder GetStandard() => new StandardNClientFactoryBuilder();
        public INClientFactoryBuilder GetCustom() => new CustomNClientFactoryBuilder();
    }
}
