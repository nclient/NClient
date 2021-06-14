using System.Threading.Tasks;
using NClient.Abstractions;
using NClient.Abstractions.Clients;
using NClient.Abstractions.HttpClients;
using NClient.Testing.Common.Entities;

namespace NClient.Testing.Common.Clients
{
    public interface IResponseClient : INClient
    {
        /// <summary>
        /// Url: api/basic?id={id}
        /// Body: empty
        /// Headers: empty
        /// </summary>
        Task<int> GetAsync(int id);

        /// <summary>
        /// Url: api/basic?id={id}
        /// Body: empty
        /// Headers: empty
        /// </summary>
        Task<HttpResponse<int>> GetResponseAsync(int id);

        /// <summary>
        /// Url: api/basic?id={id}
        /// Body: empty
        /// Headers: empty
        /// </summary>
        Task<HttpResponseWithError<int, Error>> GetResponseWithErrorAsync(int id);

        /// <summary>
        /// Url: api/basic
        /// Body: {entity}
        /// Headers: empty
        /// </summary>
        Task PostAsync(BasicEntity entity);

        /// <summary>
        /// Url: api/basic
        /// Body: {entity}
        /// Headers: empty
        /// </summary>
        Task<HttpResponse> PostResponseAsync(BasicEntity entity);

        /// <summary>
        /// Url: api/basic
        /// Body: {entity}
        /// Headers: empty
        /// </summary>
        Task<HttpResponseWithError<Error>> PostResponseWithErrorAsync(BasicEntity entity);
    }
}
