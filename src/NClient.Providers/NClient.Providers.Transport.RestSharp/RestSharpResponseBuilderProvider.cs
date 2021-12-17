using RestSharp;

namespace NClient.Providers.Transport.RestSharp
{
    public class RestSharpResponseBuilderProvider : IResponseBuilderProvider<IRestRequest, IRestResponse>
    {
        public IResponseBuilder<IRestRequest, IRestResponse> Create(IToolset toolset)
        {
            return new RestSharpResponseBuilder();
        }
    }
}
