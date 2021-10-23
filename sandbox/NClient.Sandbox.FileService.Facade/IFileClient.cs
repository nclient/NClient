using System.Threading.Tasks;
using NClient.Annotations;
using NClient.Annotations.Parameters;
using NClient.Annotations.Versioning;
using NClient.Providers.Transport;

namespace NClient.Sandbox.FileService.Facade
{
    [UseVersion("3.0")]
    [Header("client", "NClient")]
    public interface IFileClient : IFileController
    {
        [Override]
        new Task<IHttpResponse> GetTextFileAsync([RouteParam] long id);

        [Override]
        new Task<IHttpResponse> GetImageAsync(long id);
    }
}
