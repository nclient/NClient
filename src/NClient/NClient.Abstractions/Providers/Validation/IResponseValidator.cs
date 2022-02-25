using System.Threading.Tasks;
using NClient.Providers.Transport;

namespace NClient.Providers.Validation
{
    /// <summary>The response validator for validation the contents of the response received from transport.</summary>
    /// <typeparam name="TRequest">The type of request that is used in the transport implementation.</typeparam>
    /// <typeparam name="TResponse">The type of response that is used in the transport implementation.</typeparam>
    public interface IResponseValidator<TRequest, TResponse>
    {
        /// <summary>Checks success of the transport response.</summary>
        /// <param name="responseContext">The context containing transport request and response.</param>
        bool IsSuccess(IResponseContext<TRequest, TResponse> responseContext);
        
        /// <summary>The method that will be invoked if the response is unsuccessful.</summary>
        /// <param name="responseContext">The context containing transport request and response.</param>
        Task OnFailureAsync(IResponseContext<TRequest, TResponse> responseContext);
    }
    
    /// <summary>The ordinal priority response validator for validation the contents of the response received from transport.</summary>
    /// <typeparam name="TRequest">The type of request that is used in the transport implementation.</typeparam>
    /// <typeparam name="TResponse">The type of response that is used in the transport implementation.</typeparam>
    public interface IOrderedResponseValidation<TRequest, TResponse> : IResponseValidator<TRequest, TResponse>
    {
        /// <summary>Gets the response validator order. The order determines the order of response validator execution.</summary>
        public int Order { get; }
    }
}
