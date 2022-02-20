using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Extensions;
using NClient.Exceptions;
using NClient.Standalone.Tests.Clients;
using NClient.Testing.Common.Apis;
using NUnit.Framework;

namespace NClient.Tests.ClientTests
{
    [Parallelizable]
    public class TimeoutStaticClientTest
    {
        [Test, Retry(3)]
        public void Get_CustomTransportTimeout_ThrowClientValidationException()
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
                .ThrowExactly<ClientValidationException>();
        }
        
        [Test, Retry(3)]
        public async Task GetAsync_CustomTransportTimeout_ThrowClientValidationException()
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
                .ThrowExactlyAsync<ClientValidationException>();
        }
        
        [Test, Retry(3)]
        public void Get_IncreasingClientTimeout_NotThrow()
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
        public async Task GetAsync_IncreasingClientTimeout_NotThrow()
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
        public void Get_MethodStaticTimeout_NotThrow()
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
        public async Task GetAsync_InterfaceStaticTimeout_NotThrow()
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
        public void Get_MethodStaticTimeout_ThrowTaskCanceledException()
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
        public async Task GetAsync_InterfaceStaticTimeout_ThrowTaskCanceledException()
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
