using System;
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
            using var api = TimeoutApiMockFactory.MockGetMethod(id, delay: 1.Seconds());
            var httpClient = new HttpClient { Timeout = 100.Milliseconds() };
            var nclient = NClientGallery.Clients.GetCustom()
                .For<ITimeoutClientWithMetadata>(api.Urls.First())
                .UsingRestApi()
                .UsingSystemNetHttpTransport(httpClient)
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
            using var api = TimeoutApiMockFactory.MockGetMethod(id, delay: 1.Seconds());
            var httpClient = new HttpClient { Timeout = 100.Milliseconds() };
            var nclient = NClientGallery.Clients.GetCustom()
                .For<ITimeoutClientWithMetadata>(api.Urls.First())
                .UsingRestApi()
                .UsingSystemNetHttpTransport(httpClient)
                .UsingJsonSerializer()
                .Build();

            await nclient.Invoking(x => x.GetAsync(id))
                .Should()
                .ThrowExactlyAsync<TaskCanceledException>();
        }
        
        [Test]
        public void TimeoutClient_GetWithTimeout_ThrowTaskCanceledException()
        {
            const int id = 1;
            using var api = TimeoutApiMockFactory.MockGetMethod(id, delay: 1.Seconds());
            var nclient = NClientGallery.Clients.GetRest()
                .For<ITimeoutClientWithMetadata>(api.Urls.First())
                .WithTimeout(100.Milliseconds())
                .Build();
            
            nclient.Invoking(x => x.Get(id))
                .Should()
                .ThrowExactly<TaskCanceledException>();
        }
        
        [Test]
        public void TimeoutClient_GetWithTimeout_ThrowOperationCanceledException()
        {
            const int id = 1;
            using var api = TimeoutApiMockFactory.MockGetMethod(id, delay: 1.Seconds());
            var nclient = NClientGallery.Clients.GetRest()
                .For<ITimeoutClientWithMetadata>(api.Urls.First())
                .WithTimeout(0.Milliseconds())
                .Build();
            
            nclient.Invoking(x => x.Get(id))
                .Should()
                .ThrowExactly<OperationCanceledException>();
        }
        
        [Test]
        public async Task TimeoutClient_GetAsyncWithTimeout_ThrowTaskCanceledException()
        {
            const int id = 1;
            using var api = TimeoutApiMockFactory.MockGetMethod(id, delay: 1.Seconds());
            var nclient = NClientGallery.Clients.GetRest()
                .For<ITimeoutClientWithMetadata>(api.Urls.First())
                .WithTimeout(100.Milliseconds())
                .Build();

            await nclient.Invoking(x => x.GetAsync(id))
                .Should()
                .ThrowExactlyAsync<TaskCanceledException>();
        }
        
        [Test]
        public async Task TimeoutClient_GetAsyncWithTimeout_ThrowOperationCanceledException()
        {
            const int id = 1;
            using var api = TimeoutApiMockFactory.MockGetMethod(id, delay: 1.Seconds());
            var nclient = NClientGallery.Clients.GetRest()
                .For<ITimeoutClientWithMetadata>(api.Urls.First())
                .WithTimeout(0.Milliseconds())
                .Build();

            await nclient.Invoking(x => x.GetAsync(id))
                .Should()
                .ThrowExactlyAsync<OperationCanceledException>();
        }
        
        [Test]
        public void TimeoutClient_GetWithHttpClientTimeout_NotThrow()
        {
            const int id = 1;
            using var api = TimeoutApiMockFactory.MockGetMethod(id, delay: 100.Milliseconds());
            var httpClient = new HttpClient { Timeout = 200.Milliseconds() };
            var nclient = NClientGallery.Clients.GetCustom()
                .For<ITimeoutClientWithMetadata>(api.Urls.First())
                .UsingRestApi()
                .UsingSystemNetHttpTransport(httpClient)
                .UsingJsonSerializer()
                .WithTimeout(100.Milliseconds())
                .Build();
            
            nclient.Invoking(x => x.Get(id))
                .Should()
                .NotThrow();
        }
        
        [Test]
        public async Task TimeoutClient_GetAsyncWithHttpClientTimeout_NotThrow()
        {
            const int id = 1;
            using var api = TimeoutApiMockFactory.MockGetMethod(id, delay: 100.Milliseconds());
            var httpClient = new HttpClient { Timeout = 200.Milliseconds() };
            var nclient = NClientGallery.Clients.GetCustom()
                .For<ITimeoutClientWithMetadata>(api.Urls.First())
                .UsingRestApi()
                .UsingSystemNetHttpTransport(httpClient)
                .UsingJsonSerializer()
                .WithTimeout(100.Milliseconds())
                .Build();

            await nclient.Invoking(x => x.GetAsync(id))
                .Should()
                .NotThrowAsync();
        }
    }
}
