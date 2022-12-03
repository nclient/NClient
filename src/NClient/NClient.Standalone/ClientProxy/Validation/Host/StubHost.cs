using System;
using System.Threading;
using System.Threading.Tasks;
using NClient.Providers.Host;

namespace NClient.Standalone.ClientProxy.Validation.Host
{
    internal class StubHost : IHost
    {
        public Task<Uri?> TryGetUriAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
