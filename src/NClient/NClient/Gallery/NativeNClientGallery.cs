using NClient.Abstractions;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public interface INativeNClientGallery
    {
        IBasicNClientBuilder GetBasic();
        IStandardNClientBuilder GetStandard();
        INClientBuilder GetCustom();
    }
    
    public class NativeNClientGallery : INativeNClientGallery
    {
        public IBasicNClientBuilder GetBasic() => new BasicNClientBuilder();
        public IStandardNClientBuilder GetStandard() => new StandardNClientBuilder();
        public INClientBuilder GetCustom() => new CustomNClientBuilder();
    }
}
