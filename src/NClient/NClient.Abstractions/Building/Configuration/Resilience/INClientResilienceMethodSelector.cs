using System;
using System.Linq.Expressions;
using NClient.Invocation;
using NClient.Providers.Transport;

// ReSharper disable once CheckNamespace
namespace NClient
{
    /// <summary>Selector for configuring resilience for client method/methods.</summary>
    /// <typeparam name="TClient">The client type.</typeparam>
    /// <typeparam name="TRequest">The type of request that is used in the transport implementation.</typeparam>
    /// <typeparam name="TResponse">The type of response that is used in the transport implementation.</typeparam>
    public interface INClientResilienceMethodSelector<TClient, TRequest, TResponse>
    {
        /// <summary>Selects all methods of all clients.</summary>
        INClientResilienceSetter<TClient, TRequest, TResponse> ForAllMethods();
        
        /// <summary>Selects specific method of a client.</summary>
        INClientResilienceSetter<TClient, TRequest, TResponse> ForMethod(Expression<Func<TClient, Delegate>> selector);
        
        /// <summary>Selects methods by condition.</summary>
        INClientResilienceSetter<TClient, TRequest, TResponse> ForMethodsThat(Func<IMethod, IRequest, bool> condition);
    }
}
