using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Http;
using NClient.Common.Helpers;
using NClient.Models;
using NClient.Testing.Common.Apis;
using NClient.Testing.Common.Clients;
using NClient.Tests.ClientTests.Helpers;
using NUnit.Framework;

namespace NClient.Tests.ClientTests
{
    [Parallelizable]
    public class FileClientTest : ClientTestBase<IFileClientWithMetadata>
    {
        private const string DefaultContent = "response content";
        private const string DefaultName = "filename";
        private const string DefaultFileName = "filename.txt";
        private const string DefaultContentType = "application/octet-stream";
        private static readonly Encoding DefaultEncoding = Encoding.UTF32;
        private static readonly string DefaultContentDisposition = $"attachment; name={DefaultName}; filename={DefaultFileName}";

        [Test]
        public async Task GetStreamAsync_WithText_ShouldReturnStream()
        {
            var responseBytes = DefaultEncoding.GetBytes(DefaultContent);
            using var api = FileApiMockFactory.MockGetMethod(responseBytes, DefaultEncoding, DefaultContentType, DefaultContentDisposition);

            var result = await NClientGallery.Clients.GetRest().For<IFileClientWithMetadata>(host: api.Urls.First()).Build()
                .GetStreamAsync();

            using var assertionScope = new AssertionScope();
            result.Should().NotBeNull();
            result.Should().BeReadable();
            (await result.ReadToEndAsync(DefaultEncoding)).Should().Be(DefaultContent);
        }
        
        [Test]
        public async Task GetStreamContentAsync_WithText_ShouldReturnStreamContent()
        {
            var responseBytes = DefaultEncoding.GetBytes(DefaultContent);
            using var api = FileApiMockFactory.MockGetMethod(responseBytes, DefaultEncoding, DefaultContentType, DefaultContentDisposition);

            var result = await NClientGallery.Clients.GetRest().For<IFileClientWithMetadata>(host: api.Urls.First()).Build()
                .GetStreamContentAsync();

            using var assertionScope = new AssertionScope();
            result.Should().NotBeNull();
            result.Value.Should().BeReadable();
            result.Name.Should().Be(DefaultName);
            result.Encoding.Should().Be(DefaultEncoding);
            result.ContentType.Should().Be(DefaultContentType);
            (await result.Value.ReadToEndAsync(DefaultEncoding)).Should().Be(DefaultContent);
        }
        
        [Test]
        public async Task GetHttpFileContentAsync_WithText_ShouldReturnHttpFileContent()
        {
            var responseBytes = DefaultEncoding.GetBytes(DefaultContent);
            using var api = FileApiMockFactory.MockGetMethod(responseBytes, DefaultEncoding, DefaultContentType, DefaultContentDisposition);

            var result = await NClientGallery.Clients.GetRest().For<IFileClientWithMetadata>(host: api.Urls.First()).Build()
                .GetFormFileAsync();

            using var assertionScope = new AssertionScope();
            result.Should().NotBeNull();
            result.Name.Should().Be(DefaultName);
            result.FileName.Should().Be(DefaultFileName);
            result.ContentType.Should().Be(DefaultContentType);
            result.ContentDisposition.Should().Be(DefaultContentDisposition);
            var resultStream = new MemoryStream();
            await result.CopyToAsync(resultStream);
            (await resultStream.ReadToEndAsync(DefaultEncoding)).Should().Be(DefaultContent);
        }
        
        [Test]
        public async Task GetResponseWithStreamAsync_WithText_ShouldReturnStream()
        {
            var responseBytes = DefaultEncoding.GetBytes(DefaultContent);
            using var api = FileApiMockFactory.MockGetMethod(responseBytes, DefaultEncoding, DefaultContentType, DefaultContentDisposition);

            var result = await NClientGallery.Clients.GetRest().For<IFileClientWithMetadata>(host: api.Urls.First()).Build()
                .GetResponseWithStreamAsync();
            
            using var assertionScope = new AssertionScope();
            result.Should().NotBeNull();
            result.IsSuccessful.Should().BeTrue();
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Data.Should().NotBeNull();
            result.Data!.Should().BeReadable();
            (await result.Data!.ReadToEndAsync(DefaultEncoding)).Should().Be(DefaultContent);
        }
        
        [Test]
        public async Task GetResponseWithStreamContentAsync_WithText_ShouldReturnStreamContent()
        {
            var responseBytes = DefaultEncoding.GetBytes(DefaultContent);
            using var api = FileApiMockFactory.MockGetMethod(responseBytes, DefaultEncoding, DefaultContentType, DefaultContentDisposition);

            var result = await NClientGallery.Clients.GetRest().For<IFileClientWithMetadata>(host: api.Urls.First()).Build()
                .GetResponseWithStreamContentAsync();

            using var assertionScope = new AssertionScope();
            result.Should().NotBeNull();
            result.IsSuccessful.Should().BeTrue();
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Data.Should().NotBeNull();
            result.Data!.Name.Should().Be(DefaultName);
            result.Data.ContentType.Should().Be(DefaultContentType);
            result.Data.Value.Should().BeReadable();
            (await result.Data!.Value.ReadToEndAsync(DefaultEncoding)).Should().Be(DefaultContent);
        }
        
        [Test]
        public async Task GetResponseWithHttpFileContentAsync_WithText_ShouldReturnHttpFileContent()
        {
            var responseBytes = DefaultEncoding.GetBytes(DefaultContent);
            using var api = FileApiMockFactory.MockGetMethod(responseBytes, DefaultEncoding, DefaultContentType, DefaultContentDisposition);

            var result = await NClientGallery.Clients.GetRest().For<IFileClientWithMetadata>(host: api.Urls.First()).Build()
                .GetResponseWithFormFileAsync();

            using var assertionScope = new AssertionScope();
            result.Should().NotBeNull();
            result.IsSuccessful.Should().BeTrue();
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Data.Should().NotBeNull();
            result.Data!.Name.Should().Be(DefaultName);
            result.Data.FileName.Should().Be(DefaultFileName);
            result.Data.ContentType.Should().Be(DefaultContentType);
            result.Data.ContentDisposition.Should().Be(DefaultContentDisposition);
            var resultStream = new MemoryStream();
            await result.Data!.CopyToAsync(resultStream);
            (await resultStream.ReadToEndAsync(DefaultEncoding)).Should().Be(DefaultContent);
        }

        [Test]
        public async Task PostStreamContentAsync_WithMemoryStream_ShouldReturnOk()
        {
            var requestBytes = DefaultEncoding.GetBytes(DefaultContent);
            var streamContent = new StreamContent(
                new MemoryStream(requestBytes), 
                DefaultEncoding, 
                contentType: DefaultContentType);
            using var api = FileApiMockFactory.MockPostMethod(requestBytes);

            var result = await NClientGallery.Clients.GetRest().For<IFileClientWithMetadata>(host: api.Urls.First()).Build()
                .PostStreamContentAsync(streamContent);

            using var assertScope = new AssertionScope();
            result.IsSuccessful.Should().BeTrue();
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
        }
        
        [Test]
        public async Task PostHttpFileContentAsync_WithMemoryStream_ShouldReturnOk()
        {
            var requestBytes = DefaultEncoding.GetBytes(DefaultContent);
            var fileContent = new HttpFileContent(
                new MemoryStream(requestBytes), 
                name: DefaultName,
                fileName: DefaultFileName,
                contentType: DefaultContentType,
                contentDisposition: DefaultContentDisposition);
            using var api = FileApiMockFactory.MockPostMethod(requestBytes);

            var result = await NClientGallery.Clients.GetRest().For<IFileClientWithMetadata>(host: api.Urls.First()).Build()
                .PostFormFileAsync(fileContent);

            using var assertScope = new AssertionScope();
            result.IsSuccessful.Should().BeTrue();
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
        }
    }
}
