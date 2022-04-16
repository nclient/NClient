using System;
using System.Threading;
using System.Threading.Tasks;
using NClient.Providers.Transport;

namespace NClient.Providers.Mapping
{
    /// <summary>The mapper that converts transport responses into custom results.</summary>
    /// <typeparam name="TRequest">The type of request that is used in the transport implementation.</typeparam>
    /// <typeparam name="TResponse">The type of response that is used in the transport implementation.</typeparam>
    public interface IResponseMapper<TRequest, TResponse>
    {
        /// <summary>Checks if the mapper is suitable for the transformation of the response.</summary>
        /// <param name="resultType">The type of result required.</param>
        /// <param name="responseContext">The context containing transport request and response.</param>
        bool CanMap(Type resultType, IResponseContext<TRequest, TResponse> responseContext);
        
        /// <summary>Converts transport response into custom result.</summary>
        /// <param name="resultType">The type of result required.</param>
        /// <param name="responseContext">The context containing transport request and response.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        Task<object?> MapAsync(Type resultType, IResponseContext<TRequest, TResponse> responseContext, CancellationToken cancellationToken);
    }
}
