using System;
using System.Threading;
using System.Threading.Tasks;
using NClient.Providers.Transport;

namespace NClient.Providers.Api
{
    /// <summary>The request builder that turns a method call into a NClient request.</summary>
    public interface IRequestBuilder
    {
        /// <summary>Builds NClient request.</summary>
        /// <param name="requestId">The unique identifier of the request.</param>
        /// <param name="host">The address of the requested resource.</param>
        /// <param name="methodInvocation">The information about the invocation of the client's method.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        Task<IRequest> BuildAsync(Guid requestId, Uri host, IMethodInvocation methodInvocation, CancellationToken cancellationToken);
    }
}
