using Microsoft.Extensions.Logging;
using Moq;

namespace NClient.Extensions.DependencyInjection.Tests.Helpers
{
    public static class LoggerFactoryMockFactory
    {
        public static Mock<ILoggerFactory> Create<T>(Mock<ILogger<T>> loggerMock)
        {
            var loggerFactoryMock = new Mock<ILoggerFactory>();
            loggerFactoryMock.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(loggerMock.Object);
            return loggerFactoryMock;
        }

        public static void VerifyCreateLogger<T>(this Mock<ILoggerFactory> mock, Times times)
        {
            mock.Verify(x => x.CreateLogger(typeof(T).FullName), times);
        }
    }
}
