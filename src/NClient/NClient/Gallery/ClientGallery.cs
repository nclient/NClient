// ReSharper disable once CheckNamespace

namespace NClient
{
    /// <summary>The container of builder for creating pre-configured clients.</summary>
    public interface IClientGallery
    { 
        /// <summary>Gets the client builder for a REST-like web API with JSON-formatted data.</summary>
        IRestNClientBuilder GetRest();

        /// <summary>Gets the client builder used to create a custom client.</summary>
        INClientBuilder GetCustom();
    }
    
    /// <summary>The container of builder for creating pre-configured clients.</summary>
    public class ClientGallery : IClientGallery
    {
        /// <summary>Gets the client builder for a REST-like web API with JSON-formatted data.</summary>
        public IRestNClientBuilder GetRest() => new RestNClientBuilder();

        /// <summary>Gets the client builder used to create a client with custom providers.</summary>
        public INClientBuilder GetCustom() => new NClientBuilder();
    }
}
