using System.Threading.Tasks;
using NClient.Testing.Common.Entities;

namespace NClient.Testing.Common.Clients
{
    public interface IOptionalParamClient : INClient
    {
        /// <summary>
        /// Url: api/basic?id={id}
        /// Body: empty
        /// Headers: empty
        /// </summary>
        Task<int> GetAsync(int id = 1);

        /// <summary>
        /// Url: api/basic
        /// Body: {entity}
        /// Headers: empty
        /// </summary>
        Task PostAsync(BasicEntity? entity = null);
    }
}
