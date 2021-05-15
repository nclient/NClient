using System.Threading.Tasks;
using NClient.Abstractions.Clients;
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
        /// Url: api/basic
        /// Body: {entity}
        /// Headers: empty
        /// </summary>
        Task PostAsync(BasicEntity entity);
    }
}
