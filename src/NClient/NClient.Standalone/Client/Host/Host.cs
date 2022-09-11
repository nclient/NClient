using System;
using System.Threading;
using System.Threading.Tasks;
using NClient.Providers.Host;

namespace NClient.Standalone.Client.Host
{
    internal class Host : IHost
    {
        private readonly Uri? _uri;

        public Host()
        {
        }

        public Host(Uri uri)
        {
            _uri = uri;
        }

        public Task<Uri?> TryGetUriAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_uri);
        }
    }
}
