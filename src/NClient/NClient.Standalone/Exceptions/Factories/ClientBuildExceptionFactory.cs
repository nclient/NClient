using NClient.Exceptions;

namespace NClient.Standalone.Exceptions.Factories
{
    public interface IClientBuildExceptionFactory
    {
        ClientBuildException HostIsNotSet();
        ClientBuildException HttpClientIsNotSet();
        ClientBuildException SerializerIsNotSet();
    }
    
    public class ClientBuildExceptionFactory : IClientBuildExceptionFactory
    {
        public ClientBuildException HostIsNotSet() =>
            new("The client cannot be created: the host is not set.");
        
        public ClientBuildException HttpClientIsNotSet() =>
            new("The client cannot be created: the http client is not set. The step of setting an HTTP client is required.");
        
        public ClientBuildException SerializerIsNotSet() =>
            new("The client cannot be created: the http client is not set. The step of setting a serializer is required.");
    }
}
