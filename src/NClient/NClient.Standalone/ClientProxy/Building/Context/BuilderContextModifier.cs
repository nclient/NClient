namespace NClient.Standalone.ClientProxy.Building.Context
{
    internal delegate BuilderContext<TRequest, TResponse> BuildContextFunc<TRequest, TResponse>(BuilderContext<TRequest, TResponse> context);

    internal class BuilderContextModifier<TRequest, TResponse>
    {
        private BuildContextFunc<TRequest, TResponse> _buildContextFunc;
        
        public BuilderContextModifier()
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
