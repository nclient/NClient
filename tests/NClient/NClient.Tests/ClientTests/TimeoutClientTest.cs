using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Extensions;
using NClient.Providers.Api.Rest.Extensions;
using NClient.Standalone.Tests.Clients;
using NClient.Testing.Common.Apis;
using NUnit.Framework;

namespace NClient.Tests.ClientTests
{
    [Parallelizable]
    public class TimeoutClientTest
    {
        [Test]
        public void TimeoutClient_GetWithHttpClientTimeout_ThrowTaskCanceledException()
        {
            const int id = 1;
            using var api = TimeoutApiMockFactory.MockGetMethod(id);
            var httpClient = new HttpClient { Timeout = 1.Microseconds() };
            var nclient = NClientGallery.Clients.GetCustom()
                .For<ITimeoutClientWithMetadata>(api.Urls.First())
                .UsingRestApi()
                .UsingSystemHttpTransport(httpClient)
                .UsingJsonSerializer()
                .Build();

            nclient.Invoking(x => x.Get(id))
                .Should()
                .ThrowExactly<TaskCanceledException>();
        }
        
        [Test]
        public async Task TimeoutClient_GetAsyncWithHttpClientTimeout_ThrowTaskCanceledException()
        {
            const int id = 1;
            using var api = TimeoutApiMockFactory.MockGetMethod(id);
            var httpClient = new HttpClient { Timeout = 1.Microseconds() };
            var nclient = NClientGallery.Clients.GetCustom()
                .For<ITimeoutClientWithMetadata>(api.Urls.First())
                .UsingRestApi()
                .UsingSystemHttpTransport(httpClient)
                .UsingJsonSerializer()
                .Build();

            await nclient.Invoking(x => x.GetAsync(id))
                .Should()
                .ThrowExactlyAsync<TaskCanceledException>();
        }
    }
}
