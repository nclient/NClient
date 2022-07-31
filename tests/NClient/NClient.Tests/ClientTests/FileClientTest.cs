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
        private static readonly (string Key, string Value) DefaultHeader = ("Content-Language", "de-DE");

        #region Get result
        
        [Test]
        public async Task GetStreamAsync_WithText_ShouldReturnStream()
        {
            var responseBytes = DefaultEncoding.GetBytes(DefaultContent);
            using var api = FileApiMockFactory.MockGetMethod(
                responseBytes, DefaultEncoding, DefaultContentType, DefaultContentDisposition, DefaultHeader);

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
            using var api = FileApiMockFactory.MockGetMethod(
                responseBytes, DefaultEncoding, DefaultContentType, DefaultContentDisposition, DefaultHeader);

            var result = await NClientGallery.Clients.GetRest().For<IFileClientWithMetadata>(host: api.Urls.First()).Build()
                .GetStreamContentAsync();

            using var assertionScope = new AssertionScope();
            result.Should().NotBeNull();
            result.Value.Should().BeReadable();
            result.Name.Should().Be(DefaultName);
            result.Encoding.Should().Be(DefaultEncoding);
            result.ContentType.Should().Be(DefaultContentType);
            result.Metadatas.Get(DefaultHeader.Key).Should().ContainSingle();
            result.Metadatas.Get(DefaultHeader.Key).Single().Value.Should().Be(DefaultHeader.Value);
            (await result.Value.ReadToEndAsync(DefaultEncoding)).Should().Be(DefaultContent);
        }
        
        [Test]
        public async Task GetHttpFileContentAsync_WithText_ShouldReturnHttpFileContent()
        {
            var responseBytes = DefaultEncoding.GetBytes(DefaultContent);
            using var api = FileApiMockFactory.MockGetMethod(
                responseBytes, DefaultEncoding, DefaultContentType, DefaultContentDisposition, DefaultHeader);

            var result = await NClientGallery.Clients.GetRest().For<IFileClientWithMetadata>(host: api.Urls.First()).Build()
                .GetFormFileAsync();

            using var assertionScope = new AssertionScope();
            result.Should().NotBeNull();
            result.Name.Should().Be(DefaultName);
            result.FileName.Should().Be(DefaultFileName);
            result.ContentType.Should().Be(DefaultContentType);
            result.ContentDisposition.Should().Be(DefaultContentDisposition);
            result.Headers[DefaultHeader.Key].Should().ContainSingle();
            result.Headers[DefaultHeader.Key].Single().Should().Be(DefaultHeader.Value);
            var resultStream = new MemoryStream();
            await result.CopyToAsync(resultStream);
            (await resultStream.ReadToEndAsync(DefaultEncoding)).Should().Be(DefaultContent);
        }
        
        #endregion

        #region Get Response with data

        [Test]
        public async Task GetResponseWithStreamAsync_WithText_ShouldReturnStream()
        {
            var responseBytes = DefaultEncoding.GetBytes(DefaultContent);
            using var api = FileApiMockFactory.MockGetMethod(
                responseBytes, DefaultEncoding, DefaultContentType, DefaultContentDisposition, DefaultHeader);

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
            using var api = FileApiMockFactory.MockGetMethod(
                responseBytes, DefaultEncoding, DefaultContentType, DefaultContentDisposition, DefaultHeader);

            var result = await NClientGallery.Clients.GetRest().For<IFileClientWithMetadata>(host: api.Urls.First()).Build()
                .GetResponseWithStreamContentAsync();

            using var assertionScope = new AssertionScope();
            result.Should().NotBeNull();
            result.IsSuccessful.Should().BeTrue();
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Data.Should().NotBeNull();
            result.Data!.Name.Should().Be(DefaultName);
            result.Data.ContentType.Should().Be(DefaultContentType);
            result.Data.Metadatas.Get(DefaultHeader.Key).Should().ContainSingle();
            result.Data.Metadatas.Get(DefaultHeader.Key).Single().Value.Should().Be(DefaultHeader.Value);
            result.Data.Value.Should().BeReadable();
            (await result.Data!.Value.ReadToEndAsync(DefaultEncoding)).Should().Be(DefaultContent);
        }
        
        [Test]
        public async Task GetResponseWithHttpFileContentAsync_WithText_ShouldReturnHttpFileContent()
        {
            var responseBytes = DefaultEncoding.GetBytes(DefaultContent);
            using var api = FileApiMockFactory.MockGetMethod(
                responseBytes, DefaultEncoding, DefaultContentType, DefaultContentDisposition, DefaultHeader);

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
            result.Data.Headers[DefaultHeader.Key].Should().ContainSingle();
            result.Data.Headers[DefaultHeader.Key].Single().Should().Be(DefaultHeader.Value);
            var resultStream = new MemoryStream();
            await result.Data!.CopyToAsync(resultStream);
            (await resultStream.ReadToEndAsync(DefaultEncoding)).Should().Be(DefaultContent);
        }
        
        #endregion

        #region Get response with error

        [Test]
        public async Task GetErrorResponseWithStreamAsync_WithText_ShouldReturnStream()
        {
            var responseBytes = DefaultEncoding.GetBytes(DefaultContent);
            using var api = FileApiMockFactory.MockFailureGetMethod(
                responseBytes, DefaultEncoding, DefaultContentType, DefaultContentDisposition, DefaultHeader);

            var result = await NClientGallery.Clients.GetRest().For<IFileClientWithMetadata>(host: api.Urls.First()).Build()
                .GetErrorResponseWithStreamAsync();
            
            using var assertionScope = new AssertionScope();
            result.Should().NotBeNull();
            result.IsSuccessful.Should().BeFalse();
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Error.Should().NotBeNull();
            result.Error!.Should().BeReadable();
            (await result.Error!.ReadToEndAsync(DefaultEncoding)).Should().Be(DefaultContent);
        }
        
        [Test]
        public async Task GetErrorResponseWithStreamContentAsync_WithText_ShouldReturnStreamContent()
        {
            var responseBytes = DefaultEncoding.GetBytes(DefaultContent);
            using var api = FileApiMockFactory.MockFailureGetMethod(
                responseBytes, DefaultEncoding, DefaultContentType, DefaultContentDisposition, DefaultHeader);

            var result = await NClientGallery.Clients.GetRest().For<IFileClientWithMetadata>(host: api.Urls.First()).Build()
                .GetErrorResponseWithStreamContentAsync();

            using var assertionScope = new AssertionScope();
            result.Should().NotBeNull();
            result.IsSuccessful.Should().BeFalse();
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Error.Should().NotBeNull();
            result.Error!.Name.Should().Be(DefaultName);
            result.Error.ContentType.Should().Be(DefaultContentType);
            result.Error.Metadatas.Get(DefaultHeader.Key).Should().ContainSingle();
            result.Error.Metadatas.Get(DefaultHeader.Key).Single().Value.Should().Be(DefaultHeader.Value);
            result.Error.Value.Should().BeReadable();
            (await result.Error!.Value.ReadToEndAsync(DefaultEncoding)).Should().Be(DefaultContent);
        }
        
        [Test]
        public async Task GetErrorResponseWithHttpFileContentAsync_WithText_ShouldReturnHttpFileContent()
        {
            var responseBytes = DefaultEncoding.GetBytes(DefaultContent);
            using var api = FileApiMockFactory.MockFailureGetMethod(
                responseBytes, DefaultEncoding, DefaultContentType, DefaultContentDisposition, DefaultHeader);

            var result = await NClientGallery.Clients.GetRest().For<IFileClientWithMetadata>(host: api.Urls.First()).Build()
                .GetErrorResponseWithFormFileAsync();

            using var assertionScope = new AssertionScope();
            result.Should().NotBeNull();
            result.IsSuccessful.Should().BeFalse();
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Error.Should().NotBeNull();
            result.Error!.Name.Should().Be(DefaultName);
            result.Error.FileName.Should().Be(DefaultFileName);
            result.Error.ContentType.Should().Be(DefaultContentType);
            result.Error.ContentDisposition.Should().Be(DefaultContentDisposition);
            result.Error.Headers[DefaultHeader.Key].Should().ContainSingle();
            result.Error.Headers[DefaultHeader.Key].Single().Should().Be(DefaultHeader.Value);
            var resultStream = new MemoryStream();
            await result.Error!.CopyToAsync(resultStream);
            (await resultStream.ReadToEndAsync(DefaultEncoding)).Should().Be(DefaultContent);
        }
        
        #endregion

        #region Get response with data or error
        
        [Test]
        public async Task GetDataErrorResponseWithStreamAsync_WithText_ShouldReturnDataStream()
        {
            var responseBytes = DefaultEncoding.GetBytes(DefaultContent);
            using var api = FileApiMockFactory.MockGetMethod(
                responseBytes, DefaultEncoding, DefaultContentType, DefaultContentDisposition, DefaultHeader);

            var result = await NClientGallery.Clients.GetRest().For<IFileClientWithMetadata>(host: api.Urls.First()).Build()
                .GetDataErrorResponseWithStreamAsync();
            
            using var assertionScope = new AssertionScope();
            result.Should().NotBeNull();
            result.IsSuccessful.Should().BeTrue();
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Error.Should().BeNull();
            result.Data.Should().NotBeNull();
            result.Data!.Should().BeReadable();
            (await result.Data!.ReadToEndAsync(DefaultEncoding)).Should().Be(DefaultContent);
        }
        
        [Test]
        public async Task GetDataErrorResponseWithStreamContentAsync_WithText_ShouldReturnDataStreamContent()
        {
            var responseBytes = DefaultEncoding.GetBytes(DefaultContent);
            using var api = FileApiMockFactory.MockGetMethod(
                responseBytes, DefaultEncoding, DefaultContentType, DefaultContentDisposition, DefaultHeader);

            var result = await NClientGallery.Clients.GetRest().For<IFileClientWithMetadata>(host: api.Urls.First()).Build()
                .GetDataErrorResponseWithStreamContentAsync();

            using var assertionScope = new AssertionScope();
            result.Should().NotBeNull();
            result.IsSuccessful.Should().BeTrue();
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Error.Should().BeNull();
            result.Data.Should().NotBeNull();
            result.Data!.Name.Should().Be(DefaultName);
            result.Data.ContentType.Should().Be(DefaultContentType);
            result.Data.Metadatas.Get(DefaultHeader.Key).Should().ContainSingle();
            result.Data.Metadatas.Get(DefaultHeader.Key).Single().Value.Should().Be(DefaultHeader.Value);
            result.Data.Value.Should().BeReadable();
            (await result.Data!.Value.ReadToEndAsync(DefaultEncoding)).Should().Be(DefaultContent);
        }
        
        [Test]
        public async Task GetDataErrorResponseWithHttpFileContentAsync_WithText_ShouldReturnDataHttpFileContent()
        {
            var responseBytes = DefaultEncoding.GetBytes(DefaultContent);
            using var api = FileApiMockFactory.MockGetMethod(
                responseBytes, DefaultEncoding, DefaultContentType, DefaultContentDisposition, DefaultHeader);

            var result = await NClientGallery.Clients.GetRest().For<IFileClientWithMetadata>(host: api.Urls.First()).Build()
                .GetDataErrorResponseWithFormFileAsync();

            using var assertionScope = new AssertionScope();
            result.Should().NotBeNull();
            result.IsSuccessful.Should().BeTrue();
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Error.Should().BeNull();
            result.Data.Should().NotBeNull();
            result.Data!.Name.Should().Be(DefaultName);
            result.Data.FileName.Should().Be(DefaultFileName);
            result.Data.ContentType.Should().Be(DefaultContentType);
            result.Data.ContentDisposition.Should().Be(DefaultContentDisposition);
            result.Data.Headers[DefaultHeader.Key].Should().ContainSingle();
            result.Data.Headers[DefaultHeader.Key].Single().Should().Be(DefaultHeader.Value);
            var resultStream = new MemoryStream();
            await result.Data!.CopyToAsync(resultStream);
            (await resultStream.ReadToEndAsync(DefaultEncoding)).Should().Be(DefaultContent);
        }

        [Test]
        public async Task GetDataErrorResponseWithStreamAsync_WithText_ShouldReturnErrorStream()
        {
            var responseBytes = DefaultEncoding.GetBytes(DefaultContent);
            using var api = FileApiMockFactory.MockFailureGetMethod(
                responseBytes, DefaultEncoding, DefaultContentType, DefaultContentDisposition, DefaultHeader);

            var result = await NClientGallery.Clients.GetRest().For<IFileClientWithMetadata>(host: api.Urls.First()).Build()
                .GetDataErrorResponseWithStreamAsync();
            
            using var assertionScope = new AssertionScope();
            result.Should().NotBeNull();
            result.IsSuccessful.Should().BeFalse();
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Data.Should().BeNull();
            result.Error.Should().NotBeNull();
            result.Error!.Should().BeReadable();
            (await result.Error!.ReadToEndAsync(DefaultEncoding)).Should().Be(DefaultContent);
        }
        
        [Test]
        public async Task GetDataErrorResponseWithStreamContentAsync_WithText_ShouldReturnErrorStreamContent()
        {
            var responseBytes = DefaultEncoding.GetBytes(DefaultContent);
            using var api = FileApiMockFactory.MockFailureGetMethod(
                responseBytes, DefaultEncoding, DefaultContentType, DefaultContentDisposition, DefaultHeader);

            var result = await NClientGallery.Clients.GetRest().For<IFileClientWithMetadata>(host: api.Urls.First()).Build()
                .GetDataErrorResponseWithStreamContentAsync();

            using var assertionScope = new AssertionScope();
            result.Should().NotBeNull();
            result.IsSuccessful.Should().BeFalse();
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Data.Should().BeNull();
            result.Error.Should().NotBeNull();
            result.Error!.Name.Should().Be(DefaultName);
            result.Error.ContentType.Should().Be(DefaultContentType);
            result.Error.Metadatas.Get(DefaultHeader.Key).Should().ContainSingle();
            result.Error.Metadatas.Get(DefaultHeader.Key).Single().Value.Should().Be(DefaultHeader.Value);
            result.Error.Value.Should().BeReadable();
            (await result.Error!.Value.ReadToEndAsync(DefaultEncoding)).Should().Be(DefaultContent);
        }
        
        [Test]
        public async Task GetDataErrorResponseWithHttpFileContentAsync_WithText_ShouldReturnErrorHttpFileContent()
        {
            var responseBytes = DefaultEncoding.GetBytes(DefaultContent);
            using var api = FileApiMockFactory.MockFailureGetMethod(
                responseBytes, DefaultEncoding, DefaultContentType, DefaultContentDisposition, DefaultHeader);

            var result = await NClientGallery.Clients.GetRest().For<IFileClientWithMetadata>(host: api.Urls.First()).Build()
                .GetDataErrorResponseWithFormFileAsync();

            using var assertionScope = new AssertionScope();
            result.Should().NotBeNull();
            result.IsSuccessful.Should().BeFalse();
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Data.Should().BeNull();
            result.Error.Should().NotBeNull();
            result.Error!.Name.Should().Be(DefaultName);
            result.Error.FileName.Should().Be(DefaultFileName);
            result.Error.ContentType.Should().Be(DefaultContentType);
            result.Error.ContentDisposition.Should().Be(DefaultContentDisposition);
            result.Error.Headers[DefaultHeader.Key].Should().ContainSingle();
            result.Error.Headers[DefaultHeader.Key].Single().Should().Be(DefaultHeader.Value);
            var resultStream = new MemoryStream();
            await result.Error!.CopyToAsync(resultStream);
            (await resultStream.ReadToEndAsync(DefaultEncoding)).Should().Be(DefaultContent);
        }

        #endregion

        #region Post

        [Test]
        public async Task PostStreamContentAsync_WithMemoryStream_ShouldReturnOk()
        {
            var requestBytes = DefaultEncoding.GetBytes(DefaultContent);
            var streamContent = new StreamContent(
                new MemoryStream(requestBytes), 
                DefaultEncoding, 
                contentType: DefaultContentType);
            using var api = FileApiMockFactory.MockPostMethod(requestBytes, DefaultHeader);

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
            using var api = FileApiMockFactory.MockPostMethod(requestBytes, DefaultHeader);

            var result = await NClientGallery.Clients.GetRest().For<IFileClientWithMetadata>(host: api.Urls.First()).Build()
                .PostFormFileAsync(fileContent);

            using var assertScope = new AssertionScope();
            result.IsSuccessful.Should().BeTrue();
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
        }
        
        #endregion
    }
}
