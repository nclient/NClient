using System.Threading.Tasks;
using NClient.Testing.Common.Entities;

namespace NClient.Testing.Common.Clients
{
    public interface IReturnClient : INClient
    {
        BasicEntity Get(int id);
        Task<BasicEntity> GetAsync(int id);

        void Post(BasicEntity entity);
        Task PostAsync(BasicEntity entity);
    }
}
