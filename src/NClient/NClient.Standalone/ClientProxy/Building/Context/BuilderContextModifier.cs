using System.Collections.Generic;
using System.Linq;

namespace NClient.Standalone.ClientProxy.Building.Context
{
    internal delegate BuilderContext<TRequest, TResponse> BuildContextFunc<TRequest, TResponse>(BuilderContext<TRequest, TResponse> context);

    internal class BuilderContextModifier<TRequest, TResponse>
    {
        private readonly IList<BuildContextFunc<TRequest, TResponse>> _buildContextFuncs;
        
        public BuilderContextModifier()
        {
            _buildContextFuncs = new List<BuildContextFunc<TRequest, TResponse>>();
        }

        public void Add(BuildContextFunc<TRequest, TResponse> buildContextFunc)
        {
            _buildContextFuncs.Add(buildContextFunc);
        }
        
        public BuilderContext<TRequest, TResponse> Invoke(BuilderContext<TRequest, TResponse> builderContext)
        {
            return _buildContextFuncs.Aggregate(builderContext, (current, buildContextFunc) => buildContextFunc.Invoke(current));
        }
    }
}
