// ReSharper disable once CheckNamespace

namespace NClient.Providers.Handling
{
    public interface IClientHandlerProvider<TRequest, TResponse>
    {
        IClientHandler<TRequest, TResponse> Create(IToolSet toolset);
    }
}
