using System.Collections.Generic;
using System.Reflection;
using NClient.Abstractions.Customization.Resilience;
using NClient.Abstractions.Resilience;
using NClient.Builders.Context;

namespace NClient.Customization.Resilience
{
    internal class NClientResilienceSetter<TClient, TRequest, TResponse> : NClientFactoryResilienceSetter<TRequest, TResponse>, INClientResilienceSetter<TClient, TRequest, TResponse>
    {
        public NClientResilienceSetter(CustomizerContext<TRequest, TResponse> context, MethodInfo? selectedMethod) : base(context, selectedMethod)
        {
        }
        
        public NClientResilienceSetter(CustomizerContext<TRequest, TResponse> context, IEnumerable<MethodInfo> selectedMethods) : base(context, selectedMethods)
        {
        }

        INClientResilienceMethodSelector<TClient, TRequest, TResponse> INClientResilienceSetter<TClient, TRequest, TResponse>.Use(IResiliencePolicyProvider<TRequest, TResponse> resiliencePolicyProvider)
        {
            Use(resiliencePolicyProvider);
            
            return new NClientResilienceMethodSelector<TClient, TRequest, TResponse>(Context);
        }
    }
}
