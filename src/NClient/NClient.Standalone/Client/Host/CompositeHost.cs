using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NClient.Providers.Host;

namespace NClient.Standalone.Client.Host
{
    internal class CompositeHost : IHost
    {
        private readonly IReadOnlyCollection<IHost> _hosts;
        
        public CompositeHost(IReadOnlyCollection<IHost> hosts)
        {
            _hosts = hosts;
        }
        
        public async Task<Uri?> TryGetUriAsync(CancellationToken cancellationToken)
        {
            Uri lastUri = null!;
            foreach (var host in _hosts)
            {
                var uri = await host.TryGetUriAsync(cancellationToken).ConfigureAwait(false);
                if (uri is not null)
                    lastUri = uri;
            }
            return lastUri;
        }
    }
}
