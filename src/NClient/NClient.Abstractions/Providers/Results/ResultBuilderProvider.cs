using NClient.Abstractions.Providers.HttpClient;

namespace NClient.Abstractions.Providers.Results
{
    public class ResultBuilderProvider : IResultBuilderProvider<IHttpResponse>
    {
        public IResultBuilder<IHttpResponse> Create()
        {
            return new ResultBuilder();
        }
    }
}
