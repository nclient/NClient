using System.Threading.Tasks;
using NClient.Core;

namespace NClient.Testing.Common.Clients
{
    public interface IHeaderClient : INClient
    {
        /// <summary>
        /// Url: api/simple
        /// Body: empty
        /// Headers: {id}
        /// </summary>
        Task<int> GetAsync(int id);

        /// <summary>
        /// Url: api/simple?id={id}
        /// Body: empty
        /// Headers: {id}
        /// </summary>
        Task DeleteAsync(int id);
    }
}
