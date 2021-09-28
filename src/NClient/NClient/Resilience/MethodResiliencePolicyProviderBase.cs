using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;
using NClient.Annotations.Methods;
using NClient.Core.Mappers;
using Polly;
using Polly.Wrap;

namespace NClient.Resilience
{
    internal abstract class MethodResiliencePolicyProviderBase : IMethodResiliencePolicyProvider
    {
        private readonly AttributeMapper _attributeMapper;
        
        protected readonly AsyncPolicyWrap<ResponseContext> Policy;

        protected MethodResiliencePolicyProviderBase(
            int retryCount = 2,
            Func<int, TimeSpan>? sleepDurationProvider = null,
            Func<ResponseContext, bool>? resultPredicate = null)
        {
            // TODO: It is better to pass it through the constructor, but how?
            _attributeMapper = new AttributeMapper();
            
            var basePolicy = Policy<ResponseContext>.HandleResult(resultPredicate ?? (x => !x.HttpResponse.IsSuccessful)).Or<Exception>();
            var retryPolicy = basePolicy.WaitAndRetryAsync(
                retryCount,
                sleepDurationProvider ?? (retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))));
            var fallbackPolicy = basePolicy.FallbackAsync(
                fallbackAction: (delegateResult, _, _) => Task.FromResult(delegateResult.Result),
                onFallbackAsync: (delegateResult, _) =>
                {
                    if (delegateResult.Exception is not null)
                        throw delegateResult.Exception;
                    if (typeof(HttpResponse).IsAssignableFrom(delegateResult.Result.MethodInvocation.ResultType))
                        return Task.CompletedTask;
                    throw delegateResult.Result.HttpResponse.ErrorException!;
                });

            Policy = fallbackPolicy.WrapAsync(retryPolicy);
        }

        protected MethodAttribute GetMethodAttributeFor(MethodInfo methodInfo)
        {
            return (MethodAttribute)methodInfo.GetCustomAttributes()
                .Select(x => _attributeMapper.TryMap(x))
                .Single(x => x is MethodAttribute)!;
        }

        public abstract IResiliencePolicy Create(MethodInfo methodInfo);
    }
}
