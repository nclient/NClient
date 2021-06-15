using Microsoft.Extensions.Logging;

namespace NClient.Abstractions
{
    public interface IOptionalNClientBuilder<TInterface>
        : IOptionalBuilderBase<IOptionalNClientBuilder<TInterface>, TInterface>
        where TInterface : class
    {
        /// <summary>
        /// Sets custom <see cref="ILoggerFactory"/> used to create instances of <see cref="ILogger"/>.
        /// </summary>
        /// <param name="logger">The logger for a client.</param>
        IOptionalNClientBuilder<TInterface> WithLogging(ILogger<TInterface> logger);
    }
}