// ReSharper disable once CheckNamespace

namespace NClient.Providers.Validation
{
    /// <summary>The response validator provider that stores a response validator and returns it.</summary>
    public class ResponseValidatorProvider<TRequest, TResponse> : IResponseValidatorProvider<TRequest, TResponse>
    {
        private readonly IResponseValidator<TRequest, TResponse> _responseValidator;
        
        /// <summary>Initializes the response validator provider that stores a response validator and returns it.</summary>
        /// <param name="responseValidator">The validator that will be returned by the provider.</param>
        public ResponseValidatorProvider(IResponseValidator<TRequest, TResponse> responseValidator)
        {
            _responseValidator = responseValidator;
        }
        
        /// <summary>Returns the same response validator instance.</summary>
        /// <param name="toolset">Tools that help implement providers.</param>
        public IResponseValidator<TRequest, TResponse> Create(IToolset toolset)
        {
            return _responseValidator;
        }
    }
}
