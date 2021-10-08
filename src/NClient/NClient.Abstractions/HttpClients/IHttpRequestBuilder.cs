using System.Threading.Tasks;

namespace NClient.Abstractions.HttpClients
{
    // TODO: doc
    public interface IHttpMessageBuilder<TRequest>
    {
        Task<TRequest> BuildRequestAsync(IHttpRequest httpRequest);
    }
}
