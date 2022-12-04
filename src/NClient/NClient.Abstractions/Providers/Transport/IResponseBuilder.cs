using System.Threading;
using System.Threading.Tasks;

namespace NClient.Providers.Transport
{
    /// <summary>The builder for transforming a transport response to NClient response.</summary>
    /// <typeparam name="TRequest">The type of request that is used in the transport implementation.</typeparam>
    /// <typeparam name="TResponse">The type of response that is used in the transport implementation.</typeparam>
    public interface IResponseBuilder<TRequest, TResponse>
    {
        /// <summary>Builds NClient response</summary>
        /// <param name="request">The NClient request.</param>
        /// <param name="responseContext">The context containing transport request and response.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        Task<IResponse> BuildAsync(IRequest request, 
            IResponseContext<TRequest, TResponse> responseContext, 
            bool allocateMemoryForContent,
            CancellationToken cancellationToken);
    }
}
