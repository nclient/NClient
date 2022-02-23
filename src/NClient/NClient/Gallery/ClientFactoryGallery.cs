// ReSharper disable once CheckNamespace

namespace NClient
{
    /// <summary>The container of builder sets for creating pre-configured client factories.</summary>
    public interface IClientFactoryGallery
    {
        /// <summary>Gets the client builder factory for a REST-like web API with JSON-formatted data.</summary>
        INClientFactoryRestBuilder GetRest(); 
        
        /// <summary>Gets the builder used to create the client factory with custom providers.</summary>
        INClientFactoryBuilder GetCustom();
    }
    
    public class ClientFactoryGallery : IClientFactoryGallery
    {
        public INClientFactoryRestBuilder GetRest() => new NClientFactoryRestBuilder();
        public INClientFactoryBuilder GetCustom() => new NClientFactoryBuilder();
    }
}
