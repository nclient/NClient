// ReSharper disable once CheckNamespace

namespace NClient.Providers.Transport
{
    /// <summary>The context containing transport request and result.</summary>
    public interface IResponseContext<TRequest, TResponse>
    {
        /// <summary>Gets transport request.</summary>
        TRequest Request { get; }

        /// <summary>Gets transport request.</summary>
        TResponse Response { get; }
    }
}
