using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using NClient.Models;
using NClient.Providers.Transport;
using NClient.Testing.Common.Entities;

namespace NClient.Testing.Common.Clients
{
    public interface IMultipartClient
    {
        Task<IResponse> PostWithStreamAsync(BasicEntity entity, IStreamContent stream);
        Task<IResponse> PostWithFileAsync(BasicEntity entity, IFormFile file);
        Task<IResponse> PostMultipartStreamContentAsync(IStreamContent streamContent1, IStreamContent streamContent2);
        Task<IResponse> PostMultipartFormFileAsync(IFormFile formFile1, IFormFile formFile2);
    }
}
