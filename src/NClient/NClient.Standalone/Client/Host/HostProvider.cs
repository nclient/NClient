using System;
using NClient.Providers;
using NClient.Providers.Host;

namespace NClient.Standalone.Client.Host
{
    internal class HostProvider : IHostProvider
    {
        private readonly Uri? _uri;

        public HostProvider(Uri? uri)
        {
            _uri = uri;
        }
        
        public IHost Create(IToolset toolset)
        {
            return _uri is not null ? new Host(_uri) : new Host();
        }
    }
}
