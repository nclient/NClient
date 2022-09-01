using RestSharp;
using System.Threading;

namespace NClient.Providers.Transport.RestSharp
{
    /// <summary>The provider that can create builder for transforming a RestSharp response to NClient response.</summary>
    public class RestSharpResponseBuilderProvider : IResponseBuilderProvider<IRestRequest, IRestResponse>
    {
        /// <summary>Creates builder for transforming a RestSharp response to NClient response.</summary>
        /// <param name="toolset">Tools that help implement providers.</param>
        public IResponseBuilder<IRestRequest, IRestResponse> Create(IToolset toolset)
        {
            return new RestSharpResponseBuilder();
        }

        public IResponseBuilder<IRestRequest, IRestResponse> Create(IToolset toolset, CancellationTokenSource pipelineCanceller)
        {
            return new RestSharpResponseBuilder();
        }
    }
}
