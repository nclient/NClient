using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using NClient.Annotations;
using NClient.Annotations.Http;
using NClient.Models;
using NClient.Providers.Transport;
using NClient.Testing.Common.Entities;

namespace NClient.Testing.Common.Clients
{
    [Path("api/multipart")]
    public interface IMultipartClientWithMetadata : IMultipartClient
    {
        [PostMethod]
        new Task<IResponse> PostWithStreamAsync([FormParam] BasicEntity entity, [FormParam] IStreamContent stream);

        [PostMethod]
        new Task<IResponse> PostWithFileAsync([FormParam] BasicEntity entity, [FormParam] IFormFile file);
        
        [PostMethod]
        new Task<IResponse> PostMultipartStreamContentAsync([BodyParam] IStreamContent streamContent1, [BodyParam] IStreamContent streamContent2);
        
        [PostMethod]
        new Task<IResponse> PostMultipartFormFileAsync([BodyParam] IFormFile formFile1, [BodyParam] IFormFile formFile2);
    }
}
