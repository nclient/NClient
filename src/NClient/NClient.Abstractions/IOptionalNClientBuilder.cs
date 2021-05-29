using Microsoft.Extensions.Logging;

namespace NClient.Abstractions
{
    public interface IOptionalNClientBuilder<TInterface> 
        : ICustomBuilderBase<IOptionalNClientBuilder<TInterface>, TInterface>
        where TInterface : class
    {
        IOptionalNClientBuilder<TInterface> WithLogging(ILogger<TInterface> logger);
    }
}