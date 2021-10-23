using System.Net;
using System.Threading.Tasks;
using NClient.Annotations;
using NClient.Annotations.Methods;
using NClient.Providers.Transport;
using NClient.Testing.Common.Clients;
using NClient.Testing.Common.Entities;

namespace NClient.Standalone.Tests.Clients
{
    [Path("api/response")]
    public interface IResponseClientWithMetadata : IResponseClient
    {
        [GetMethod]
        [Response(typeof(int), HttpStatusCode.OK)]
        [Response(typeof(string), HttpStatusCode.BadRequest)]
        [Response(typeof(void), HttpStatusCode.InternalServerError)]
        new Task<int> GetAsync(int id);

        [GetMethod]
        [Response(typeof(int), HttpStatusCode.OK)]
        [Response(typeof(string), HttpStatusCode.BadRequest)]
        [Response(typeof(void), HttpStatusCode.InternalServerError)]
        new Task<IHttpResponse<int>> GetResponseAsync(int id);

        [GetMethod]
        [Response(typeof(int), HttpStatusCode.OK)]
        [Response(typeof(string), HttpStatusCode.BadRequest)]
        [Response(typeof(void), HttpStatusCode.InternalServerError)]
        new Task<IHttpResponseWithError<int, HttpError>> GetResponseWithErrorAsync(int id);

        [PostMethod]
        [Response(typeof(void), HttpStatusCode.OK)]
        [Response(typeof(string), HttpStatusCode.BadRequest)]
        [Response(typeof(void), HttpStatusCode.InternalServerError)]
        new Task PostAsync(BasicEntity entity);

        [PostMethod]
        [Response(typeof(void), HttpStatusCode.OK)]
        [Response(typeof(string), HttpStatusCode.BadRequest)]
        [Response(typeof(void), HttpStatusCode.InternalServerError)]
        new Task<IHttpResponse> PostResponseAsync(BasicEntity entity);

        [PostMethod]
        [Response(typeof(void), HttpStatusCode.OK)]
        [Response(typeof(string), HttpStatusCode.BadRequest)]
        [Response(typeof(void), HttpStatusCode.InternalServerError)]
        new Task<IHttpResponseWithError<HttpError>> PostResponseWithErrorAsync(BasicEntity entity);
    }
}
