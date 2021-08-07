using System.Threading.Tasks;
using NClient.Abstractions.HttpClients;
using NClient.Annotations;
using NClient.Annotations.Methods;
using NClient.Annotations.Parameters;
using NClient.Annotations.Versioning;

namespace NClient.Sandbox.FileService.Facade
{
    [UseVersion("3.0")]
    [Header("client", "NClient")]
    public interface IFileClient : IFileController
    {
        [GetMethod("textFiles/{id}")]
        new Task<HttpResponse> GetTextFileAsync([RouteParam] long id);
        
        [GetMethod("images/{id}")]
        new Task<HttpResponse> GetImageAsync(long id);
    }
}
