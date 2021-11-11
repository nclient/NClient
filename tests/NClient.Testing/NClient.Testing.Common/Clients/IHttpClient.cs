using System.Threading.Tasks;
using NClient.Providers.Transport;
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
        Task<IResponse<int>> GetAsync(int id);

        /// <summary>
        /// Url: api/basic
        /// Body: {entity}
        /// Headers: empty
        /// </summary>
        Task<IResponse<BasicEntity>> PostAsync(BasicEntity entity);

        /// <summary>
        /// Url: api/basic
        /// Body: {entity}
        /// Headers: empty
        /// </summary>
        Task<IResponse> PutAsync(BasicEntity entity);

        /// <summary>
        /// Url: api/basic?id={id}
        /// Body: empty
        /// Headers: empty
        /// </summary>
        IResponse Delete(int id);
    }
}
