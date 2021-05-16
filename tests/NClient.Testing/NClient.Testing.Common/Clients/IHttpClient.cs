using System.Threading.Tasks;
using NClient.Abstractions.HttpClients;
using NClient.Testing.Common.Entities;

namespace NClient.Testing.Common.Clients
{
    public interface IHttpClient
    {
        /// <summary>
        /// Url: api/basic?id={id}
        /// Body: empty
        /// Headers: empty
        /// </summary>
        Task<HttpValueResponse<int>> GetAsync(int id);

        /// <summary>
        /// Url: api/basic
        /// Body: {entity}
        /// Headers: empty
        /// </summary>
        Task<HttpValueResponse<BasicEntity>> PostAsync(BasicEntity entity);

        /// <summary>
        /// Url: api/basic
        /// Body: {entity}
        /// Headers: empty
        /// </summary>
        Task<HttpResponse> PutAsync(BasicEntity entity);

        /// <summary>
        /// Url: api/basic?id={id}
        /// Body: empty
        /// Headers: empty
        /// </summary>
        HttpResponse Delete(int id);
    }
}