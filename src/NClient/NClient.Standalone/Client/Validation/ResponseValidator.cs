using System.Collections.Generic;
using System.Linq;
using NClient.Abstractions.Ensuring;
using NClient.Abstractions.Resilience;

namespace NClient.Standalone.Client.Validation
{
    internal interface IResponseValidator<TRequest, TResponse>
    {
        bool IsValid(IResponseContext<TRequest, TResponse> responseContext);
        IResponseContext<TRequest, TResponse> Ensure(IResponseContext<TRequest, TResponse> responseContext);
    }
    
    internal class ResponseValidator<TRequest, TResponse> : IResponseValidator<TRequest, TResponse>
    {
        private readonly IReadOnlyCollection<IEnsuringSettings<TRequest, TResponse>> _ensuringSettings;
        
        public ResponseValidator(IEnumerable<IEnsuringSettings<TRequest, TResponse>> ensuringSettings)
        {
            _ensuringSettings = ensuringSettings.ToArray();
        }

        public bool IsValid(IResponseContext<TRequest, TResponse> responseContext)
        {
            return _ensuringSettings.All(x => x.IsSuccess(responseContext));
        }
        
        public IResponseContext<TRequest, TResponse> Ensure(IResponseContext<TRequest, TResponse> responseContext)
        {
            foreach (var ensuringSetting in _ensuringSettings)
            {
                if (ensuringSetting.IsSuccess(responseContext))
                    continue;
                
                ensuringSetting.OnFailure(responseContext);
                return responseContext;
            }
            
            return responseContext;
        }
    }
}
