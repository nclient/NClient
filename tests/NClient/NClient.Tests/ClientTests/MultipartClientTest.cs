using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Http;
using NClient.Models;
using NClient.Testing.Common.Apis;
using NClient.Testing.Common.Clients;
using NClient.Testing.Common.Entities;
using NClient.Tests.ClientTests.Helpers;
using NUnit.Framework;

namespace NClient.Tests.ClientTests
{
    [Parallelizable]
    public class MultipartClientTest : ClientTestBase<IMultipartClientWithMetadata>
    {
        private const string DefaultContent = "response content";
        private const string DefaultName = "filename";
        private const string DefaultFileName = "filename.txt";
        private const string DefaultContentType = "application/octet-stream";
        private static readonly Encoding DefaultEncoding = Encoding.UTF8;
        private static readonly string DefaultContentDisposition = $"attachment; name={DefaultName}; filename={DefaultFileName}";
        private static readonly (string Key, string Value) DefaultHeader = ("Content-Language", "de-DE");

        [Test]
        public async Task PostWithStreamAsync_WithCustomObjectAndStream_ShouldReturnOk()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            var keyValues = new Dictionary<string, object>
            {
                [nameof(BasicEntity.Id)] = entity.Id,
                [nameof(BasicEntity.Value)] = entity.Value
            };
            var requestBytes = DefaultEncoding.GetBytes(DefaultContent);
            var streamContent = new StreamContent(
                new MemoryStream(requestBytes), 
                DefaultEncoding, 
                contentType: DefaultContentType);
            using var api = MultipartApiMockFactory.MockPostMethod(keyValues, requestBytes);

            var result = await NClientGallery.Clients.GetRest().For<IMultipartClientWithMetadata>(host: api.Urls.First()).Build()
                .PostWithStreamAsync(entity, streamContent);

            result.StatusCode.Should().Be(StatusCodes.Status200OK);
        }
        
        [Test]
        public async Task PostWithFileAsync_WithCustomObjectAndFile_ShouldReturnOk()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            var keyValues = new Dictionary<string, object>
            {
                [nameof(BasicEntity.Id)] = entity.Id,
                [nameof(BasicEntity.Value)] = entity.Value
            };
            var requestBytes = DefaultEncoding.GetBytes(DefaultContent);
            var fileContent = new HttpFileContent(
                new MemoryStream(requestBytes), 
                name: DefaultName,
                fileName: DefaultFileName,
                contentType: DefaultContentType,
                contentDisposition: DefaultContentDisposition);
            using var api = MultipartApiMockFactory.MockPostMethod(keyValues, requestBytes);

            var result = await NClientGallery.Clients.GetRest().For<IMultipartClientWithMetadata>(host: api.Urls.First()).Build()
                .PostWithFileAsync(entity, fileContent);

            result.StatusCode.Should().Be(StatusCodes.Status200OK);
        }
        
        [Test]
        public async Task PostMultipartStreamContentAsync_WithTwoMemoryStream_ShouldReturnOk()
        {
            var requestBytes1 = DefaultEncoding.GetBytes(DefaultContent);
            var streamContent1 = new StreamContent(
                new MemoryStream(requestBytes1), 
                DefaultEncoding, 
                contentType: DefaultContentType);
            var requestBytes2 = DefaultEncoding.GetBytes(DefaultContent);
            var streamContent2 = new StreamContent(
                new MemoryStream(requestBytes2), 
                DefaultEncoding, 
                contentType: DefaultContentType);
            using var api = MultipartApiMockFactory.MockPostMethod(requestBytes1, requestBytes2, DefaultHeader);

            var result = await NClientGallery.Clients.GetRest().For<IMultipartClientWithMetadata>(host: api.Urls.First()).Build()
                .PostMultipartStreamContentAsync(streamContent1, streamContent2);

            using var assertScope = new AssertionScope();
            result.IsSuccessful.Should().BeTrue();
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
        }
        
        [Test]
        public async Task PostMultipartHttpFileContentAsync_WithTwoMemoryStream_ShouldReturnOk()
        {
            var requestBytes1 = DefaultEncoding.GetBytes(DefaultContent);
            var fileContent1 = new HttpFileContent(
                new MemoryStream(requestBytes1), 
                name: DefaultName,
                fileName: DefaultFileName,
                contentType: DefaultContentType,
                contentDisposition: DefaultContentDisposition);
            var requestBytes2 = DefaultEncoding.GetBytes(DefaultContent);
            var fileContent2 = new HttpFileContent(
                new MemoryStream(requestBytes2), 
                name: DefaultName,
                fileName: DefaultFileName,
                contentType: DefaultContentType,
                contentDisposition: DefaultContentDisposition);
            using var api = MultipartApiMockFactory.MockPostMethod(requestBytes1, requestBytes2, DefaultHeader);

            var result = await NClientGallery.Clients.GetRest().For<IMultipartClientWithMetadata>(host: api.Urls.First()).Build()
                .PostMultipartFormFileAsync(fileContent1, fileContent2);

            using var assertScope = new AssertionScope();
            result.IsSuccessful.Should().BeTrue();
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
        }
    }
}
