using System;
using System.Linq.Expressions;
using NClient.Invocation;
using NClient.Providers.Transport;

// ReSharper disable once CheckNamespace
namespace NClient
{
    /// <summary>Selector for configuring resilience for client method/methods.</summary>
    /// <typeparam name="TRequest">The type of request that is used in the transport implementation.</typeparam>
    /// <typeparam name="TResponse">The type of response that is used in the transport implementation.</typeparam>
    public interface INClientFactoryResilienceMethodSelector<TRequest, TResponse>
    {
        /// <summary>Selects all methods of all clients.</summary>
        INClientFactoryResilienceSetter<TRequest, TResponse> ForAllMethods();

        /// <summary>Selects all methods of specific client.</summary>
        /// <typeparam name="TClient">The type of client.</typeparam>
        INClientFactoryResilienceSetter<TRequest, TResponse> ForAllMethodsOf<TClient>();
        
        /// <summary>Selects specific method of a client.</summary>
        /// <typeparam name="TClient">The type of client.</typeparam>
        INClientFactoryResilienceSetter<TRequest, TResponse> ForMethodOf<TClient>(Expression<Func<TClient, Delegate>> selector);
        
        /// <summary>Selects methods by condition.</summary>
        INClientFactoryResilienceSetter<TRequest, TResponse> ForMethodsThat(Func<IMethod, IRequest, bool> condition);
    }
}
