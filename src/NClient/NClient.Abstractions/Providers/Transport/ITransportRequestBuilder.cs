using System.Threading;
using System.Threading.Tasks;

// ReSharper disable once UnusedTypeParameter
namespace NClient.Providers.Transport
{
    /// <summary>The builder for transforming a NClient request to transport request.</summary>
    /// <typeparam name="TRequest">The type of request that is used in the transport implementation.</typeparam>
    /// <typeparam name="TResponse">The type of response that is used in the transport implementation.</typeparam>
    public interface ITransportRequestBuilder<TRequest, TResponse>
    {
        /// <summary>Builds transport request.</summary>
        /// <param name="request">The container for data used to make requests.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        Task<TRequest> BuildAsync(IRequest request, CancellationToken cancellationToken);
    }
}
