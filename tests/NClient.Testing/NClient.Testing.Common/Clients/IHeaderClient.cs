using System.Threading.Tasks;
using NClient.Abstractions;
using NClient.Abstractions.Clients;

namespace NClient.Testing.Common.Clients
{
    public interface IHeaderClient : INClient
    {
        /// <summary>
        /// Url: api/header
        /// Body: empty
        /// Headers: {id}
        /// </summary>
        Task<int> GetAsync(int id);

        /// <summary>
        /// Url: api/header
        /// Body: empty
        /// Headers: {id}
        /// </summary>
        Task DeleteAsync(int id);
    }
}
