using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using NClient.Annotations;
using NClient.Annotations.Http;
using NClient.Models;
using NClient.Providers.Transport;

namespace NClient.Testing.Common.Clients
{
    [Path("api/file")]
    public interface IFileClientWithMetadata : IFileClient
    {
        [GetMethod]
        new Task<Stream> GetStreamAsync();
        
        [GetMethod]
        new Task<IStreamContent> GetStreamContentAsync();
        
        [GetMethod]
        new Task<IFormFile> GetFormFileAsync();
        
        [GetMethod]
        new Task<IResponse<Stream>> GetResponseWithStreamAsync();
        
        [GetMethod]
        new Task<IResponse<IStreamContent>> GetResponseWithStreamContentAsync();
        
        [GetMethod]
        new Task<IResponse<IFormFile>> GetResponseWithFormFileAsync();
        
        [PostMethod]
        new Task<IResponse> PostStreamContentAsync(IStreamContent streamContent);
        
        [PostMethod]
        new Task<IResponse> PostFormFileAsync(IFormFile formFile);
    }
}
