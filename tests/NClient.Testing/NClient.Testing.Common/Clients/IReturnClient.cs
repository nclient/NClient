using System.Threading.Tasks;
using NClient.Abstractions.Clients;
using NClient.Testing.Common.Entities;

namespace NClient.Testing.Common.Clients
{
    public interface IReturnClient : INClient
    {
        /// <summary>
        /// Url: api?id={id}
        /// Body: empty
        /// Headers: empty
        /// </summary>
        Task<BasicEntity> GetAsync(int id);

        /// <summary>
        /// Url: api?id={id}
        /// Body: empty
        /// Headers: empty
        /// </summary>
        BasicEntity Get(int id);

        /// <summary>
        /// Url: api
        /// Body: {entity}
        /// Headers: empty
        /// </summary>
        Task PostAsync(BasicEntity entity);

        /// <summary>
        /// Url: api
        /// Body: {entity}
        /// Headers: empty
        /// </summary>
        void Post(BasicEntity entity);
    }
}
