using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Extensions;
using NClient.Exceptions;
using NClient.Testing.Common.Apis;
using NClient.Testing.Common.Clients;
using NClient.Testing.Common.Helpers;
using NUnit.Framework;

namespace NClient.Tests.ClientTests
{
    [Parallelizable]
    public class TimeoutClientTest
    {
        [Test, Retry(3)]
        public void Get_CustomTransportTimeout_ThrowClientValidationException()
        {
            const int id = 1;
            using var api = TimeoutApiMockFactory.MockGetMethod(id, delay: 1.Seconds());
            var httpClient = new HttpClient { Timeout = 100.Milliseconds() };
            var nclient = NClientGallery.Clients.GetCustom()
                .For<ITimeoutClientWithMetadata>(api.Urls.First().ToUri())
                .UsingRestApi()
                .UsingSystemNetHttpTransport(httpClient)
                .UsingJsonSerializer()
                .Build();
            
            nclient.Invoking(x => x.Get(id))
                .Should()
                .ThrowExactly<ClientValidationException>();
        }
        
        [Test, Retry(3)]
        public async Task GetAsync_CustomTransportTimeout_ThrowClientValidationException()
        {
            const int id = 1;
            using var api = TimeoutApiMockFactory.MockGetMethod(id, delay: 1.Seconds());
            var httpClient = new HttpClient { Timeout = 100.Milliseconds() };
            var nclient = NClientGallery.Clients.GetCustom()
                .For<ITimeoutClientWithMetadata>(api.Urls.First().ToUri())
                .UsingRestApi()
                .UsingSystemNetHttpTransport(httpClient)
                .UsingJsonSerializer()
                .Build();

            await nclient.Invoking(x => x.GetAsync(id))
                .Should()
                .ThrowExactlyAsync<ClientValidationException>();
        }
        
        [Test, Retry(3)]
        public void Get_DefaultTransportTimeoutWithClientTimeout_ThrowTaskCanceledException()
        {
            const int id = 1;
            using var api = TimeoutApiMockFactory.MockGetMethod(id, delay: 1.Seconds());
            var nclient = NClientGallery.Clients.GetRest()
                .For<ITimeoutClientWithMetadata>(api.Urls.First().ToUri())
                .WithTimeout(100.Milliseconds())
                .Build();
            
            nclient.Invoking(x => x.Get(id))
                .Should()
                .ThrowExactly<TaskCanceledException>();
        }
        
        [Test, Retry(3)]
        public void Get_DefaultTransportTimeoutWithClientTimeout_ThrowOperationCanceledException()
        {
            const int id = 1;
            using var api = TimeoutApiMockFactory.MockGetMethod(id, delay: 1.Seconds());
            var nclient = NClientGallery.Clients.GetRest()
                .For<ITimeoutClientWithMetadata>(api.Urls.First().ToUri())
                .WithHandling(new DelayClientHandler(200.Milliseconds()))
                .WithTimeout(100.Milliseconds())
                .Build();
            
            nclient.Invoking(x => x.Get(id))
                .Should()
                .ThrowExactly<OperationCanceledException>();
        }
        
        [Test, Retry(3)]
        public async Task GetAsync_DefaultTransportTimeoutWithClientTimeout_ThrowTaskCanceledException()
        {
            const int id = 1;
            using var api = TimeoutApiMockFactory.MockGetMethod(id, delay: 1.Seconds());
            var nclient = NClientGallery.Clients.GetRest()
                .For<ITimeoutClientWithMetadata>(api.Urls.First().ToUri())
                .WithTimeout(100.Milliseconds())
                .Build();

            await nclient.Invoking(x => x.GetAsync(id))
                .Should()
                .ThrowExactlyAsync<TaskCanceledException>();
        }
        
        [Test, Retry(3)]
        public async Task GetAsync_DefaultTransportTimeoutWithClientTimeout_ThrowOperationCanceledException()
        {
            const int id = 1;
            using var api = TimeoutApiMockFactory.MockGetMethod(id, delay: 1.Seconds());
            var nclient = NClientGallery.Clients.GetRest()
                .For<ITimeoutClientWithMetadata>(api.Urls.First().ToUri())
                .WithHandling(new DelayClientHandler(200.Milliseconds()))
                .WithTimeout(100.Milliseconds())
                .Build();

            await nclient.Invoking(x => x.GetAsync(id))
                .Should()
                .ThrowExactlyAsync<OperationCanceledException>();
        }
        
        [Test, Retry(3)]
        public void Get_CustomTransportTimeoutWithClientTimeout_ThrowClientValidationException()
        {
            const int id = 1;
            using var api = TimeoutApiMockFactory.MockGetMethod(id, delay: 100.Milliseconds());
            var httpClient = new HttpClient { Timeout = 200.Milliseconds() };
            var nclient = NClientGallery.Clients.GetCustom()
                .For<ITimeoutClientWithMetadata>(api.Urls.First().ToUri())
                .UsingRestApi()
                .UsingSystemNetHttpTransport(httpClient)
                .UsingJsonSerializer()
                .WithTimeout(100.Milliseconds())
                .Build();
            
            nclient.Invoking(x => x.Get(id))
                .Should()
                .ThrowExactly<ClientValidationException>();
        }
        
        [Test, Retry(3)]
        public async Task GetAsync_CustomTransportTimeoutWithClientTimeout_ThrowClientValidationException()
        {
            const int id = 1;
            using var api = TimeoutApiMockFactory.MockGetMethod(id, delay: 100.Milliseconds());
            var httpClient = new HttpClient { Timeout = 200.Milliseconds() };
            var nclient = NClientGallery.Clients.GetCustom()
                .For<ITimeoutClientWithMetadata>(api.Urls.First().ToUri())
                .UsingRestApi()
                .UsingSystemNetHttpTransport(httpClient)
                .UsingJsonSerializer()
                .WithTimeout(100.Milliseconds())
                .Build();

            await nclient.Invoking(x => x.GetAsync(id))
                .Should()
                .ThrowExactlyAsync<ClientValidationException>();
        }
    }
}
