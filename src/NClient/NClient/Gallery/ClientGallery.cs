using NClient.Abstractions;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public interface IClientGallery
    {
        IBasicNClientBuilder GetBasic();
        IStandardNClientBuilder GetStandard();
        INClientBuilder GetCustom();
    }
    
    public class ClientGallery : IClientGallery
    {
        public IBasicNClientBuilder GetBasic() => new BasicNClientBuilder();
        public IStandardNClientBuilder GetStandard() => new StandardNClientBuilder();
        public INClientBuilder GetCustom() => new CustomNClientBuilder();
    }
}
