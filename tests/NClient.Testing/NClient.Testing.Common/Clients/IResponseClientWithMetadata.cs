using System.Net;
using System.Threading.Tasks;
using NClient.Annotations;
using NClient.Annotations.Http;
using NClient.Providers.Transport;
using NClient.Testing.Common.Entities;

namespace NClient.Testing.Common.Clients
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
        new Task<IResponse<int>> GetResponseAsync(int id);

        [GetMethod]
        [Response(typeof(int), HttpStatusCode.OK)]
        [Response(typeof(string), HttpStatusCode.BadRequest)]
        [Response(typeof(void), HttpStatusCode.InternalServerError)]
        new Task<IResponseWithError<int, HttpError>> GetResponseWithErrorAsync(int id);

        [PostMethod]
        [Response(typeof(void), HttpStatusCode.OK)]
        [Response(typeof(string), HttpStatusCode.BadRequest)]
        [Response(typeof(void), HttpStatusCode.InternalServerError)]
        new Task PostAsync(BasicEntity entity);

        [PostMethod]
        [Response(typeof(void), HttpStatusCode.OK)]
        [Response(typeof(string), HttpStatusCode.BadRequest)]
        [Response(typeof(void), HttpStatusCode.InternalServerError)]
        new Task<IResponse> PostResponseAsync(BasicEntity entity);

        [PostMethod]
        [Response(typeof(void), HttpStatusCode.OK)]
        [Response(typeof(string), HttpStatusCode.BadRequest)]
        [Response(typeof(void), HttpStatusCode.InternalServerError)]
        new Task<IResponseWithError<HttpError>> PostResponseWithErrorAsync(BasicEntity entity);
    }
}
