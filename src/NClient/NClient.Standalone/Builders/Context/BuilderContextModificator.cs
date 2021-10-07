namespace NClient.Standalone.Builders.Context
{
    internal delegate BuilderContext<TRequest, TResponse> BuildContextFunc<TRequest, TResponse>(BuilderContext<TRequest, TResponse> context);

    internal class BuilderContextModificator<TRequest, TResponse>
    {
        private BuildContextFunc<TRequest, TResponse> _buildContextFunc;
        
        public BuilderContextModificator()
        {
            _buildContextFunc = context => context;
        }

        public void Add(BuildContextFunc<TRequest, TResponse> buildContextFunc)
        {
            _buildContextFunc += buildContextFunc;
        }
        
        public BuilderContext<TRequest, TResponse> Invoke(BuilderContext<TRequest, TResponse> builderContext)
        { 
            return _buildContextFunc(builderContext);
        }
    }
}
