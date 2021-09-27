using NClient.Abstractions;
using NClient.Testing.Common.Entities;

namespace NClient.Testing.Common.Clients
{
    public interface ISyncClient : INClient
    {
        /// <summary>
        /// Url: api/simple?id={id}
        /// Body: empty
        /// Headers: empty
        /// </summary>
        public int Get(int id);

        /// <summary>
        /// Url: api/simple
        /// Body: {entity}
        /// Headers: empty
        /// </summary>
        public void Post(BasicEntity entity);

        /// <summary>
        /// Url: api/simple
        /// Body: {entity}
        /// Headers: empty
        /// </summary>
        public void Put(BasicEntity entity);

        /// <summary>
        /// Url: api/simple?id={id}
        /// Body: empty
        /// Headers: empty
        /// </summary>
        public void Delete(int id);
    }
}
