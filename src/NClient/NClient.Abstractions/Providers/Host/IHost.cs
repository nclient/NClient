using System;
using System.Threading;
using System.Threading.Tasks;

namespace NClient.Providers.Host
{
    /// <summary>
    /// Provides the interface for retrieving host URI for client requests
    /// </summary>
    public interface IHost
    {
        /// <summary>
        /// Returns a host uri for client requests
        /// </summary>
        public Task<Uri?> TryGetUriAsync(CancellationToken cancellationToken = default);
    }
}
