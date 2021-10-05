using NClient.Abstractions;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public interface INativeNClientFactoryGallery
    {
        IBasicNClientFactoryBuilder GetBasic();
        IStandardNClientFactoryBuilder GetStandard();
        INClientFactoryBuilder GetCustom();
    }
    
    public class NativeNClientFactoryGallery : INativeNClientFactoryGallery
    {
        public IBasicNClientFactoryBuilder GetBasic() => new BasicNClientFactoryBuilder();
        public IStandardNClientFactoryBuilder GetStandard() => new StandardNClientFactoryBuilder();
        public INClientFactoryBuilder GetCustom() => new CustomNClientFactoryBuilder();
    }
}
