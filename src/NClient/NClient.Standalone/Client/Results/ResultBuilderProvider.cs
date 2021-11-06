using NClient.Providers.Results;

namespace NClient.Standalone.Client.Results
{
    internal class ResultBuilderProvider<TRequest, TResponse> : IResultBuilderProvider<TRequest, TResponse>
    {
        private readonly IResultBuilder<TRequest, TResponse> _resultBuilder;
        
        public ResultBuilderProvider(IResultBuilder<TRequest, TResponse> resultBuilder)
        {
            _resultBuilder = resultBuilder;
        }
        
        public IResultBuilder<TRequest, TResponse> Create()
        {
            return _resultBuilder;
        }
    }
}
