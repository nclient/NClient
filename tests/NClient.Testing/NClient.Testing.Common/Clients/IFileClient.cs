using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using NClient.Models;
using NClient.Providers.Transport;

namespace NClient.Testing.Common.Clients
{
    public interface IFileClient
    {
        Task<Stream> GetStreamAsync();
        Task<IStreamContent> GetStreamContentAsync();
        Task<IFormFile> GetFormFileAsync();
        
        Task<IResponse<Stream>> GetResponseWithStreamAsync();
        Task<IResponse<IStreamContent>> GetResponseWithStreamContentAsync();
        Task<IResponse<IFormFile>> GetResponseWithFormFileAsync();
        
        Task<IResponseWithError<Stream>> GetErrorResponseWithStreamAsync();
        Task<IResponseWithError<IStreamContent>> GetErrorResponseWithStreamContentAsync();
        Task<IResponseWithError<IFormFile>> GetErrorResponseWithFormFileAsync();
        
        Task<IResponseWithError<Stream, Stream>> GetDataErrorResponseWithStreamAsync();
        Task<IResponseWithError<IStreamContent, IStreamContent>> GetDataErrorResponseWithStreamContentAsync();
        Task<IResponseWithError<IFormFile, IFormFile>> GetDataErrorResponseWithFormFileAsync();

        Task<IResponse> PostStreamContentAsync(IStreamContent streamContent);
        Task<IResponse> PostFormFileAsync(IFormFile formFile);
    }
}
