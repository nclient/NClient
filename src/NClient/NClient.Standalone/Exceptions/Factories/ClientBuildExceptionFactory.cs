using NClient.Exceptions;

namespace NClient.Standalone.Exceptions.Factories
{
    internal interface IClientBuildExceptionFactory
    {
        ClientBuildException HostIsNotSet();
        ClientBuildException ApiIsNotSet();
        ClientBuildException TransportIsNotSet();
        ClientBuildException SerializerIsNotSet();
    }
    
    internal class ClientBuildExceptionFactory : IClientBuildExceptionFactory
    {
        public ClientBuildException HostIsNotSet() =>
            new("The client cannot be created: the host is not set.");

        public ClientBuildException ApiIsNotSet() =>
            new("The client cannot be created: the api settings is not set. The step of setting an api is required.");

        public ClientBuildException TransportIsNotSet() =>
            new("The client cannot be created: the transport is not set. The step of setting an transport is required.");
        
        public ClientBuildException SerializerIsNotSet() =>
            new("The client cannot be created: the http client is not set. The step of setting a serializer is required.");
    }
}
