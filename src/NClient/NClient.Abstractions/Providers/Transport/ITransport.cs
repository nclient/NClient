using System;
using System.Threading;
using System.Threading.Tasks;

namespace NClient.Providers.Transport
{
    /// <summary>Invoker of requests.</summary>
    public interface ITransport<TRequest, TResponse>
    {
        /// <summary>Gets the timespan to wait before the request times out.</summary>
        public TimeSpan Timeout { get; }
        
        /// <summary>Executes requests.</summary>
        /// <param name="transportRequest">The container for request data.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        Task<TResponse> ExecuteAsync(TRequest transportRequest, CancellationToken cancellationToken);
    }
}
