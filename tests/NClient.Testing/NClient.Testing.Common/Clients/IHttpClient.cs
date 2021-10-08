using System.Threading.Tasks;
using NClient.Providers.Results.HttpMessages;
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
        Task<IHttpResponse<int>> GetAsync(int id);

        /// <summary>
        /// Url: api/basic
        /// Body: {entity}
        /// Headers: empty
        /// </summary>
        Task<IHttpResponse<BasicEntity>> PostAsync(BasicEntity entity);

        /// <summary>
        /// Url: api/basic
        /// Body: {entity}
        /// Headers: empty
        /// </summary>
        Task<IHttpResponse> PutAsync(BasicEntity entity);

        /// <summary>
        /// Url: api/basic?id={id}
        /// Body: empty
        /// Headers: empty
        /// </summary>
        IHttpResponse Delete(int id);
    }
}
