using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Extensions;
using NClient.Standalone.Tests.Clients;
using NClient.Testing.Common.Apis;
using NUnit.Framework;

namespace NClient.Tests.ClientTests
{
    [Parallelizable]
    public class CancellationClientTest
    {
        [Test]
        public void CancellationClient_GetWithoutCancellation_IntInBody()
        {
            const int id = 1;
            using var api = CancellationApiMockFactory.MockGetMethod(id);

            var result = NClientGallery.Clients.GetRest().For<ICancellationClientWithMetadata>(api.Urls.First()).Build()
                .Get(id, CancellationToken.None);

            result.Should().Be(id);
        }
        
        [Test]
        public async Task CancellationClient_GetWithCancellation_ThrowTaskCanceledException()
        {
            const int id = 1;
            using var api = CancellationApiMockFactory.MockGetMethodWithDelay(id);
            var source = new CancellationTokenSource();
            var cancellationTask = Task.Run(async () =>
            {
                await Task.Delay(1.Seconds(), CancellationToken.None);
                source.Cancel();
            }, CancellationToken.None);
            
            NClientGallery.Clients.GetRest().For<ICancellationClientWithMetadata>(api.Urls.First()).Build()
                .Invoking(client => client.Get(id, source.Token))
                .Should()
                .ThrowExactly<TaskCanceledException>();
            await cancellationTask;
        }
        
        [Test]
        public async Task CancellationClient_GetAsyncWithoutCancellation_IntInBody()
        {
            const int id = 1;
            using var api = CancellationApiMockFactory.MockGetMethod(id);

            var result = await NClientGallery.Clients.GetRest().For<ICancellationClientWithMetadata>(api.Urls.First()).Build()
                .GetAsync(id, CancellationToken.None);

            result.Should().Be(id);
        }
        
        [Test]
        public async Task CancellationClient_GetAsyncWithCancellation_ThrowTaskCanceledException()
        {
            const int id = 1;
            using var api = CancellationApiMockFactory.MockGetMethodWithDelay(id);
            var source = new CancellationTokenSource();
            
            var resultTask = NClientGallery.Clients.GetRest().For<ICancellationClientWithMetadata>(api.Urls.First()).Build()
                .GetAsync(id, source.Token);
            source.Cancel();
            
            await resultTask.Invoking(x => x)
                .Should()
                .ThrowExactlyAsync<TaskCanceledException>();
        }
    }
}
