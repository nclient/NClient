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
    public class TimeoutStaticClientTest
    {
        [Test, Retry(3)]
        public void TimeoutStaticClient_GetWithHttpClientTimeout_NotThrow()
        {
            const int id = 1;
            using var api = TimeoutApiMockFactory.MockGetMethod(id, delay: 1.Seconds());
            var httpClient = new HttpClient { Timeout = 2.Seconds() };
            var nclient = NClientGallery.Clients.GetCustom()
                .For<ITimeoutStaticClientWithMetadata>(api.Urls.First())
                .UsingRestApi()
                .UsingSystemNetHttpTransport(httpClient)
                .UsingJsonSerializer()
                .Build();
            
            nclient.Invoking(x => x.Get(id))
                .Should()
                .ThrowExactly<TaskCanceledException>();
        }
        
        [Test, Retry(3)]
        public async Task TimeoutStaticClient_GetAsyncWithHttpClientTimeout_NotThrow()
        {
            const int id = 1;
            using var api = TimeoutApiMockFactory.MockGetMethod(id, delay: 1.Seconds());
            var httpClient = new HttpClient { Timeout = 2.Seconds() };
            var nclient = NClientGallery.Clients.GetCustom()
                .For<ITimeoutStaticClientWithMetadata>(api.Urls.First())
                .UsingRestApi()
                .UsingSystemNetHttpTransport(httpClient)
                .UsingJsonSerializer()
                .Build();

            await nclient.Invoking(x => x.GetAsync(id))
                .Should()
                .ThrowExactlyAsync<TaskCanceledException>();
        }
        
        [Test, Retry(3)]
        public void TimeoutStaticClient_GetWithTimeout_NotThrow()
        {
            const int id = 1;
            using var api = TimeoutApiMockFactory.MockGetMethod(id, delay: 1.Seconds());
            var nclient = NClientGallery.Clients.GetRest()
                .For<ITimeoutStaticClientWithMetadata>(api.Urls.First())
                .WithTimeout(2.Seconds())
                .Build();
            
            nclient.Invoking(x => x.Get(id))
                .Should()
                .NotThrow();
        }

        [Test, Retry(3)]
        public async Task TimeoutStaticClient_GetAsyncWithTimeout_NotThrow()
        {
            const int id = 1;
            using var api = TimeoutApiMockFactory.MockGetMethod(id, delay: 1.Seconds());
            var nclient = NClientGallery.Clients.GetRest()
                .For<ITimeoutStaticClientWithMetadata>(api.Urls.First())
                .WithTimeout(2.Seconds())
                .Build();

            await nclient.Invoking(x => x.GetAsync(id))
                .Should()
                .NotThrowAsync();
        }
        
        [Test, Retry(3)]
        public void TimeoutStaticClient_GetWithStaticTimeout_NotThrow()
        {
            const int id = 1;
            using var api = TimeoutApiMockFactory.MockGetMethod(id, delay: 1.Microseconds());
            var nclient = NClientGallery.Clients.GetRest()
                .For<ITimeoutStaticClientWithMetadata>(api.Urls.First())
                .Build();
            
            nclient.Invoking(x => x.Get(id))
                .Should()
                .NotThrow();
        }

        [Test, Retry(3)]
        public async Task TimeoutStaticClient_GetAsyncWithStaticTimeout_NotThrow()
        {
            const int id = 1;
            using var api = TimeoutApiMockFactory.MockGetMethod(id, delay: 1.Microseconds());
            var nclient = NClientGallery.Clients.GetRest()
                .For<ITimeoutStaticClientWithMetadata>(api.Urls.First())
                .Build();

            await nclient.Invoking(x => x.GetAsync(id))
                .Should()
                .NotThrowAsync();
        }
        
        [Test, Retry(3)]
        public void TimeoutStaticClient_GetWithStaticTimeout_ThrowTaskCanceledException()
        {
            const int id = 1;
            using var api = TimeoutApiMockFactory.MockGetMethod(id, delay: 1.Seconds());
            var nclient = NClientGallery.Clients.GetRest()
                .For<ITimeoutStaticClientWithMetadata>(api.Urls.First())
                .Build();
            
            nclient.Invoking(x => x.Get(id))
                .Should()
                .ThrowExactly<TaskCanceledException>();
        }
        
        [Test, Retry(3)]
        public async Task TimeoutStaticClient_GetAsyncWithStaticTimeout_ThrowTaskCanceledException()
        {
            const int id = 1;
            using var api = TimeoutApiMockFactory.MockGetMethod(id, delay: 1.Seconds());
            var nclient = NClientGallery.Clients.GetRest()
                .For<ITimeoutStaticClientWithMetadata>(api.Urls.First())
                .Build();

            await nclient.Invoking(x => x.GetAsync(id))
                .Should()
                .ThrowExactlyAsync<TaskCanceledException>();
        }
        
        [Test, Retry(3)]
        public void TimeoutStaticClient_GetWithStaticMethodTimeout_AndNotThrow()
        {
            const int id = 1;
            using var api = TimeoutApiMockFactory.MockGetMethod(id, delay: 0.5.Seconds());
            var nclient = NClientGallery.Clients.GetRest()
                .For<ITimeoutStaticClientWithMetadata>(api.Urls.First())
                .Build();
            
            nclient.Invoking(x => x.Get(id))
                .Should()
                .NotThrow();
        }

        [Test, Retry(3)]
        public async Task TimeoutStaticClient_GetAsyncWithStaticInterfaceTimeout_ThrowTaskCanceledException()
        {
            const int id = 1;
            using var api = TimeoutApiMockFactory.MockGetMethod(id, delay: 0.5.Seconds());
            var nclient = NClientGallery.Clients.GetRest()
                .For<ITimeoutStaticClientWithMetadata>(api.Urls.First())
                .Build();

            await nclient.Invoking(x => x.GetAsync(id))
                .Should()
                .ThrowExactlyAsync<TaskCanceledException>();
        }
    }
}
