using System.Threading.Tasks;
using NClient.Abstractions.Clients;
using NClient.Testing.Common.Entities;

namespace NClient.Testing.Common.Clients
{
    public interface IQueryClient : INClient
    {
        /// <summary>
        /// Url: api/simple?id={id}
        /// Body: empty
        /// Headers: empty
        /// </summary>
        Task<int> GetAsync(int id);

        /// <summary>
        /// Url: api/simple?entity.id={id}
        /// Body: empty
        /// Headers: empty
        /// </summary>
        Task PostAsync(BasicEntity entity);

        /// <summary>
        /// Url: api/simple?entity.id={id}
        /// Body: empty
        /// Headers: empty
        /// </summary>
        Task PutAsync(BasicEntity entity);

        /// <summary>
        /// Url: api/simple?id={id}
        /// Body: empty
        /// Headers: empty
        /// </summary>
        Task DeleteAsync(int id);
    }
}
