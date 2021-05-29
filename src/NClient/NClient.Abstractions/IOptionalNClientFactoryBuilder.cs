using Microsoft.Extensions.Logging;

namespace NClient.Abstractions
{
    public interface IOptionalNClientFactoryBuilder 
        : ICustomBuilderBase<IOptionalNClientFactoryBuilder, INClientFactory>
    {
        IOptionalNClientFactoryBuilder WithLogging(ILoggerFactory loggerFactory);
    }
}