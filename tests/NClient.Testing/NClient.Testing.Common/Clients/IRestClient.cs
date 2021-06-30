using System.Threading.Tasks;
using NClient.Abstractions;
using NClient.Abstractions.Clients;
using NClient.Testing.Common.Entities;

namespace NClient.Testing.Common.Clients
{
    public interface IRestClient : INClient
    {
        /// <summary>
        /// Url: api/simple/{id}
        /// Body: empty
        /// Headers: empty
        /// </summary>
        Task<int> GetAsync(int id);

        /// <summary>
        /// Url: api/simple/{id}
        /// Body: empty
        /// Headers: empty
        /// </summary>
        Task<string> GetAsync(string id);

        /// <summary>
        /// Url: api/simple
        /// Body: {entity}
        /// Headers: empty
        /// </summary>
        Task PostAsync(BasicEntity entity);

        /// <summary>
        /// Url: api/simple
        /// Body: {entity}
        /// Headers: empty
        /// </summary>
        Task PutAsync(BasicEntity entity);

        /// <summary>
        /// Url: api/simple/{id}
        /// Body: empty
        /// Headers: empty
        /// </summary>
        Task DeleteAsync(int id);
    }
}
