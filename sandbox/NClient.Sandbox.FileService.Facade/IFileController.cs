using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NClient.Annotations;
using NClient.Annotations.Methods;
using NClient.Annotations.Parameters;
using NClient.Annotations.Versioning;

namespace NClient.Sandbox.FileService.Facade
{
    [Api]
    [Path("api/nclient/v{version:apiVersion}/[controller]")]
    [Version("1.0"), Version("2.0"), Version("3.0")]
    public interface IFileController
    {
        [GetMethod("textFiles/{id}")]
        [Response(typeof(string), HttpStatusCode.OK)]
        [Response(typeof(void), HttpStatusCode.BadRequest)]
        [Response(typeof(void), HttpStatusCode.NotFound)]
        Task<IActionResult> GetTextFileAsync([RouteParam] long id);

        [PostMethod("textFiles")]
        [Response(typeof(void), HttpStatusCode.BadRequest)]
        Task PostTextFileAsync(byte[] fileBytes);

        [GetMethod("images/{id}")]
        [Response(typeof(byte[]), HttpStatusCode.OK)]
        [Response(typeof(void), HttpStatusCode.BadRequest)]
        [Response(typeof(void), HttpStatusCode.NotFound)]
        Task<IActionResult> GetImageAsync(long id);

        [PostMethod("images")]
        [Response(typeof(void), HttpStatusCode.BadRequest)]
        Task PostImageFileAsync(byte[] fileBytes);
    }
}
