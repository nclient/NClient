using System.Threading.Tasks;

namespace NClient.Testing.Common.Clients
{
    public class ClassClient
    {
        public Task<int> GetAsync(int id)
        {
            return Task.FromResult(1);
        }
    }
}
