using System.Threading.Tasks;
using NClient.Providers.Transport;

// ReSharper disable once CheckNamespace
namespace NClient.Providers.Validation
{
    /// <summary>The customizable response validator for validation the contents of the response received from transport.</summary>
    public class ResponseValidator<TRequest, TResponse> : IResponseValidator<TRequest, TResponse>
    {
        private readonly IResponseValidatorSettings<TRequest, TResponse> _responseValidatorSettings;
        
        /// <summary>Initializes the customizable response validator for validation the contents of the response received from transport.</summary>
        /// <param name="responseValidatorSettings">The settings used to validate the response.</param>
        public ResponseValidator(IResponseValidatorSettings<TRequest, TResponse> responseValidatorSettings)
        {
            _responseValidatorSettings = responseValidatorSettings;
        }
        
        /// <summary>Checks success of a response.</summary>
        /// <param name="responseContext">The context containing transport request and result.</param>
        public bool IsSuccess(IResponseContext<TRequest, TResponse> responseContext)
        {
            return _responseValidatorSettings.IsSuccess(responseContext);
        }

        /// <summary>Invokes specific action if the response is unsuccessful.</summary>
        /// <param name="responseContext">The context containing transport request and result.</param>
        public Task OnFailureAsync(IResponseContext<TRequest, TResponse> responseContext)
        {
            _responseValidatorSettings.OnFailure(responseContext);
            return Task.CompletedTask;
        }
    }
}
