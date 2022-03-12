using System;
using Microsoft.Extensions.Logging;
using Moq;

namespace NClient.Extensions.DependencyInjection.Tests.Helpers
{
    public static class LoggerMockFactory
    {
        public static Mock<ILogger<T>> Create<T>()
        {
            var loggerScopeMock = new Mock<IDisposable>();
            loggerScopeMock.Setup(x => x.Dispose());
            var loggerMock = new Mock<ILogger<T>>();
            loggerMock.Setup(x => x.BeginScope(It.IsAny<It.IsAnyType>())).Returns(loggerScopeMock.Object);
            loggerMock.Setup(x => x.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>) It.IsAny<object>()));

            return loggerMock;
        }

        public static void VerifyLog<T>(this Mock<ILogger<T>> mock, Times times)
        {
            mock.Verify(x => x.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>) It.IsAny<object>()),
                times);
        }
    }
}
